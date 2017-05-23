using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MessageSysDataManagementLib;
using System.Threading;
using System.IO;

namespace MessageSystemCSServer
{
    class Server
    {
        const int PORT = 6666;
        private static TcpListener listener;
        private static List<ClientData> clients = new List<ClientData>();

        static void Main(string[] args)
        {
            clients = new List<ClientData>();

            Console.Title = "MessageSystemCS | Server";

            StartServer();
        }

        private static void StartServer()
        {
            Console.WriteLine("Starting server on " + Packet.GetThisIPv4Adress());

            listener = new TcpListener(new IPEndPoint(IPAddress.Parse(Packet.GetThisIPv4Adress()), PORT));

            Console.WriteLine("Server started. Waiting for new Client connections...\n");

            Thread listenForNewClients = new Thread(ListenForNewClients);
            listenForNewClients.Start();
        }

        private static void ListenForNewClients()
        {
            listener.Start();

            while (true)
            {
                //Sobald ein Client sich verbinden will ClientData erstellen und in ClientListe schieben
                clients.Add(new ClientData(listener.AcceptTcpClient()));
                Console.WriteLine("New Client connected");
            }            
        }

        public static void DataIn(object tcpClient)          //clientData)
        {
            TcpClient client = (TcpClient)tcpClient;
            NetworkStream clientStream = client.GetStream();
            try
            {
                while (true)
                {
                    byte[] buffer; //Daten
                    byte[] dataSize = new byte[4]; //Länge

                    int readBytes = clientStream.Read(dataSize, 0, 4);

                    while (readBytes != 4)
                    {
                        readBytes += clientStream.Read(dataSize, readBytes, 4 - readBytes);
                    }
                    var contentLength = BitConverter.ToInt32(dataSize, 0);

                    buffer = new byte[contentLength];
                    readBytes = 0;
                    while (readBytes != buffer.Length)
                    {
                        readBytes += clientStream.Read(buffer, readBytes, buffer.Length - readBytes);
                    }

                    //Daten sind im Buffer-Array
                    DataManagerForIncommingClientData(new Packet(buffer), client);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("No: " + ex.ErrorCode + " Message: " + ex.Message);
            }
            catch (IOException)
            {
                ClientData disconnectedClient = GetClientFromList(client);
                Console.WriteLine("Client disconnected with UID: " + GetClientFromList(client).UID);
                clients.Remove(disconnectedClient);
                Console.WriteLine("Client removed from list.\n");

                //Notify other Clients that client has disconnected.
                foreach (ClientData c in clients)
                {
                    c.SendDataPacketToClient(new Packet(Packet.PacketType.ClientDisconnected, disconnectedClient.UID + ";" + disconnectedClient.PublicKey));
                    Console.WriteLine(c.UID + " notified that " + disconnectedClient.UID + " has disconnected");
                }
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private static void DataManagerForIncommingClientData(Packet p, TcpClient clientSocket)
        {
            ClientData client;
            switch (p.type)
            {
                case Packet.PacketType.Registration:
                    Console.WriteLine("Client wants to register with UID: " + p.uid + " and Public-Key: " + p.publicKey);
                    client = GetClientFromList(clientSocket);

                    foreach (ClientData c in clients)
                    {
                        if(c.UID.ToLower() == p.uid.ToLower())
                        {
                            client.SendDataPacketToClient(new Packet(Packet.PacketType.RegistrationFail, "User with this uid already exists!"));
                        }
                    }
                    client.UID = p.uid;
                    client.PublicKey = p.publicKey;
                    client.SendDataPacketToClient(new Packet(Packet.PacketType.RegistrationSuccess));

                    //Notify clients that new Client has connected
                    foreach (ClientData c in clients)
                    {
                        if (c.UID != p.uid)
                        {
                            c.SendDataPacketToClient(new Packet(Packet.PacketType.ClientConnected, p.uid + ";" + p.publicKey));
                        }
                    }
                    break;
                case Packet.PacketType.GetClientList:
                    client = GetClientFromList(clientSocket);
                    Console.WriteLine("Client " + client.UID + " wants Client List. Generating...");

                    List<object> dataList = new List<object>();
                    foreach (ClientData c in clients)
                    {
                        if (c.UID != client.UID)
                        {
                            dataList.Add(c.UID + ";" + c.PublicKey);
                        }
                    }
                    client.SendDataPacketToClient(new Packet(Packet.PacketType.ClientList, dataList));
                    break;
                case Packet.PacketType.Message:
                    Console.WriteLine("Incomming Message from " + p.uid + " at " + p.messageTimeStamp.ToString("HH:mm:ss") + " to " + p.destinationUID + " data: " + Encoding.UTF8.GetString(p.messageData));
                    foreach (ClientData c in clients)
                    {
                        if(c.UID == p.destinationUID)
                        {
                            c.SendDataPacketToClient(new Packet(Packet.PacketType.Message, p.messageTimeStamp, p.uid, p.destinationUID, p.messageData));
                            Console.WriteLine("Message send to " + c.UID);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Findet den passenden Client welcher über diesen Socket mit dem Server verbunden ist.
        /// </summary>
        /// <param name="clientSocket">Socket mit dem der Client mit dem Server verbunden ist</param>
        /// <returns>Gefundenen Client andernfalls null.</returns>
        private static ClientData GetClientFromList(TcpClient tcpClient)
        {
            foreach (ClientData client in clients)
            {
                if (client.TcpClient == tcpClient)
                {
                    return client;
                }
            }

            return null;
        }

        /// <summary>
        /// Findet den passenden Client welcher mit dieser UID mit dem Server verbunden ist.
        /// </summary>
        /// <param name="uid">Client UID</param>
        /// <returns>Gefundenen Client andernfalls null.</returns>
        private static ClientData GetClientFromList(string uid)
        {
            foreach (ClientData client in clients)
            {
                if (client.UID == uid)
                {
                    return client;
                }
            }

            return null;
        }
    }
}
