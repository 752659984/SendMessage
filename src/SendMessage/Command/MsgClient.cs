using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public class MsgClient
    {
        public delegate void DoServer(Socket socket);
        public DoServer DoServerHandler;

        public Socket ClientSocket { get; }

        public int ServerPort { get; }

        public string ServerIP { get; }

        public MsgClient(int serverPort, string serverIP)
        {
            ServerPort = serverPort;
            ServerIP = serverIP;
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
            ClientSocket.Connect(endPoint);
        }

        //public void Receive()
        //{
        //    while (true)
        //    {
        //        var client = ClientSocket.Accept();
        //        Task.Run(() => { DoServerHandler?.Invoke(client); });
        //    }
        //}
    }
}
