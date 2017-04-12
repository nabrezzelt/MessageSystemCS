using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MessageSysDataManagementLib;
using System.Threading;

namespace MessageSystemCSServer
{
    class Server
    {
        const int PORT = 6666;
        private static Socket listenerSocket;
        private static List<ClientData> clients;

        static void Main(string[] args)
        {
            clients = new List<ClientData>();
            StartServer();
        }

        private static void StartServer()
        {
            Console.WriteLine("Starting server on " + Packet.GetThisIPv4Adress());

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(Packet.GetThisIPv4Adress()), PORT); //Erstelle einen Endpunkt auf den sich die Clients verbinden können

            listenerSocket.Bind(localEndPoint);

            Console.WriteLine("Server started. Waiting for new Client connections...\n");

            Thread listenForNewClients = new Thread(ListenForClients);
            listenForNewClients.Start();
        }

        private static void ListenForClients()
        {
            while (true)
            {
                listenerSocket.Listen(0);

                //Sobald ein Client sich verbinden will ClientData erstellen und in ClientListe schieben
                clients.Add(new ClientData(listenerSocket.Accept()));
                Console.WriteLine("New Client connected to the server.");
            }
        }

        public static void DataIn(object cSocket)          //clientData)
        {
            //Socket clientSocket = ((ClientData) clientData).clientSocket;
            //string uid = ((ClientData)clientData).uid;

            Socket clientSocket = (Socket)cSocket;

            byte[] buffer;
            int readBytes;

            while (true)
            {
                try
                {
                    buffer = new byte[16000000]; //ca. 16MB
                    readBytes = clientSocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        //Verarbeite erhaltene Daten(byte[]) und gebe diese als Packet an den DatenManager weiter
                        Packet p = new Packet(buffer); //Konstruktor von "Packet" kann einen byte[] annehmen und erstellt daraus ein passendes Paket
                        DataManagerForIncommingClientData(p, clientSocket);
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("[" + ex.ErrorCode + "] " + ex.Message);

                    if (ex.ErrorCode == 10054)
                    {
                        ClientData client = GetClientFromList(clientSocket);
                        Console.WriteLine("Client disconnected with UID: " + GetClientFromList(clientSocket).uid);
                        clients.Remove(client);
                        Console.WriteLine("Client removed from list.\n");

                        //Notify other Clients that client has disconnected.
                        foreach (ClientData c in clients)
                        {
                            c.SendDataPacketToClient(new Packet(Packet.PacketType.ClientDisconnected, client.uid + ";" + client.publicKey));
                            Console.WriteLine(c.uid + " notified that " + client.uid + " has disconnected");
                        }
                    }

                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }
        }

        private static void DataManagerForIncommingClientData(Packet p, Socket clientSocket)
        {
            ClientData client;
            switch (p.type)
            {
                case Packet.PacketType.Registration:
                    Console.WriteLine("Client registered with UID: " + p.uid + " and Public-Key: " + p.publicKey);
                    client = GetClientFromList(clientSocket);
                    client.uid = p.uid;
                    client.publicKey = p.publicKey;
                    client.SendDataPacketToClient(new Packet(Packet.PacketType.RegistrationSuccess));

                    //Notify clients that new Client has connected
                    foreach (ClientData c in clients)
                    {
                        if (c.uid != p.uid)
                        {
                            c.SendDataPacketToClient(new Packet(Packet.PacketType.ClientConnected, p.uid + ";" + p.publicKey));
                        }
                    }
                    break;
                case Packet.PacketType.GetClientList:
                    client = GetClientFromList(clientSocket);
                    Console.WriteLine("Client " + client.uid + " wants Client List. Generating...");

                    List<object> dataList = new List<object>();
                    foreach (ClientData c in clients)
                    {
                        if (c.uid != client.uid)
                        {
                            dataList.Add(c.uid + ";" + c.publicKey);
                        }
                    }
                    client.SendDataPacketToClient(new Packet(Packet.PacketType.ClientList, dataList));
                    break;
                case Packet.PacketType.Message:
                    Console.WriteLine("Incomming Message from " + p.uid + " to " + p.destinationUID + " data: " + Encoding.UTF8.GetString(p.messageData));
                    foreach (ClientData c in clients)
                    {
                        if(c.uid == p.destinationUID)
                        {
                            c.SendDataPacketToClient(new Packet(Packet.PacketType.Message, p.messageTimeStamp, p.uid, p.destinationUID, p.messageData));
                            Console.WriteLine("Message send to " + c.uid);
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
        private static ClientData GetClientFromList(Socket clientSocket)
        {
            foreach (ClientData client in clients)
            {
                if (client.clientSocket == clientSocket)
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
                if (client.uid == uid)
                {
                    return client;
                }
            }

            return null;
        }
    }
}
