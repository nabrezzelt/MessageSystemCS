using MessageSysDataManagementLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageSystemCSServer
{
    class ClientData
    {
        public Socket clientSocket;
        private Thread clientThread;
        public string publicKey;
        public string uid;

        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;

            //Starte für jeden Client nach dem Verbinden einen seperaten Thread in dem auf neue eingehende Nachrichten gehört/gewartet wird.
            clientThread = new Thread(Server.DataIn);
            clientThread.Start(clientSocket);
        }

        public void SendDataPacketToClient(Packet p)
        {
            clientSocket.Send(p.ConvertToBytes());
        }
    }
}
