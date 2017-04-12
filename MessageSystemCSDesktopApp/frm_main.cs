using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageSysDataManagementLib;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace MessageSystemCSDesktopApp
{
    public partial class frm_main : Form
    {
        const int PORT = 6666;
        private Socket serverSocket;
        private string uid;
        private string publicKey;
        private string privateKey;

        public frm_main()
        {            
            InitializeComponent();

            Tuple<string, string> keyPair = KeyManagement.CreateKeyPair();
            privateKey = keyPair.Item1;
            publicKey = keyPair.Item2;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            uid = tb_uid.Text;

            ConnectToServer(tb_ip.Text, PORT);

            //Register            
            SendDataToServer(new Packet(Packet.PacketType.Registration, uid, publicKey));
            InvokeGUIThread(() => { tb_received_messages.Text += ">> Registration-Packet sent.\n"; });

            //Starte Thread zum Empfangen von Daten
            Thread receiveDataThread = new Thread(DataIn);
            receiveDataThread.Start();

            btn_connect.Enabled = false;
        }

        public void SendDataToServer(Packet packet)
        {
            serverSocket.Send(packet.ConvertToBytes());
        }

        private void DataManagerForIncommingServerPackets(Packet p)
        {
            switch (p.type)
            {               
                case Packet.PacketType.RegistrationSuccess:
                    InvokeGUIThread(() => { tb_received_messages.Text += ">> Registration was successfull.\n"; });
                    GetClientist();
                    break;
                case Packet.PacketType.ClientList:
                    InvokeGUIThread(() => {
                        tb_received_messages.Text += ">> ClientList received.\n";

                        foreach (object clientdata in p.data)
                        {
                            string[] data = ((string) clientdata).Split(';');
                            lb_clients.Items.Add(new LocalClientData(data[0], data[1]));
                        }
                    });
                    break;
                case Packet.PacketType.ClientConnected:
                    InvokeGUIThread(() => {
                        tb_received_messages.Text += ">> New Client connected.\n";
                        
                        string[] data = ((string)p.singleStringData).Split(';');
                        lb_clients.Items.Add(new LocalClientData(data[0], data[1]));                       
                    });
                    break;
                case Packet.PacketType.ClientDisconnected:
                    InvokeGUIThread(() => {

                        string[] data = (p.singleStringData).Split(';');
                        string packetDataUID = data[0];
                        string packetDataPublicKey = data[1];

                        foreach (ConversationTabPage conversation in tc_conversations.TabPages)
                        {
                            if(conversation.UID == packetDataUID && conversation.PublicKey == packetDataPublicKey)
                            {
                                tc_conversations.TabPages.Remove(conversation);
                            }
                        }

                        foreach (LocalClientData item in lb_clients.Items)
                        {
                            if(packetDataUID == item.uid && packetDataPublicKey == item.publicKey)
                            {
                                lb_clients.Items.Remove(item);
                                break;
                            }
                        }
                        
                    });
                    break;
                case Packet.PacketType.Message:
                    Log("Incomming Message from "+ p.uid);
                    break;
            }
        }

        private void ConnectToServer(string ip, int port)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            while (!serverSocket.Connected)
            {
                try
                {
                    InvokeGUIThread(() => { tb_received_messages.Text += ">> Trying to connect at " + ip + " on port " + port + "...\n"; });
                    serverSocket.Connect(serverEndPoint);                    
                    InvokeGUIThread(() => { tb_received_messages.Text += ">> Connected\n"; });

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);
                }
            }

        }

        private void DataIn()
        {
            byte[] buffer;
            int readBytes;

            while (true)
            {
                try
                {
                    buffer = new byte[16000000]; //ca 16MB
                    readBytes = serverSocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        DataManagerForIncommingServerPackets(new Packet(buffer));
                    }
                }
                catch (SocketException)
                {
                    MessageBox.Show("The server has disconnected!");
                    serverSocket.Dispose();
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }

        private void InvokeGUIThread(Action action)
        {
            Invoke(action);
        }

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        public void GetClientist()
        {
            SendDataToServer(new Packet(Packet.PacketType.GetClientList));
        }

        private void lb_clients_DoubleClick(object sender, EventArgs e)
        {
            if (lb_clients.SelectedItem != null)
            {
                foreach (ConversationTabPage conversation  in tc_conversations.TabPages)
                {
                    if(conversation.UID == ((LocalClientData)lb_clients.SelectedItem).uid && conversation.PublicKey == ((LocalClientData)lb_clients.SelectedItem).publicKey)
                    {
                        tb_received_messages.Text += ">> Conversation already exists.\n";
                        tc_conversations.SelectedTab = conversation;
                        return;
                    }
                }

                tc_conversations.TabPages.Add(new ConversationTabPage(this, ((LocalClientData)lb_clients.SelectedItem).uid, ((LocalClientData)lb_clients.SelectedItem).publicKey));
                tc_conversations.SelectedTab = tc_conversations.TabPages[tc_conversations.TabPages.Count-1];
            }
        }

        public void Log(string message)
        {
            InvokeGUIThread(() => { tb_received_messages.Text += ">> " + message + "\n"; });
        }       
        
        public void SendMessage(string destinationID, byte[] encrypedMessage)
        {
            SendDataToServer(new Packet(Packet.PacketType.Message, DateTime.Now, uid, destinationID, encrypedMessage));
        }
    }
}
