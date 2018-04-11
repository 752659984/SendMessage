using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public class MsgService
    {
        public delegate void DoClient(ClientEntity client);

        public DoClient DoClientHandler;

        public static object LockObject = new object();

        public int MaxConnectionLength { get; }

        public int Port { get; }

        public bool IsListen { get; set; }
        
        public List<ClientEntity> ClientsSocket { get; set; }

        public Socket ServerSocket { get; set; }

        public MsgService(int maxConnLen, int port)
        {
            MaxConnectionLength = maxConnLen;
            Port = port;
            IsListen = true;
            ClientsSocket = new List<ClientEntity>(maxConnLen);
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new IPEndPoint(IPAddress.Any, Port);
            ServerSocket.Bind(endPoint);
            ServerSocket.Listen(maxConnLen);
        }

        public void Receive()
        {
            while (IsListen)
            {
                var client = ServerSocket.Accept();
                if (ClientsSocket.Count < MaxConnectionLength)
                {
                    var c = new ClientEntity() { ClientSocket = client, LastActiveTime = DateTime.Now };
                    ClientsSocket.Add(c);
                    Task.Run(() => { DoClientHandler?.Invoke(c); });
                    //DoClientHandler?.Invoke(client);
                }
                else
                {
                    client.Close();
                }
            }
        }

        public bool LogOut(Socket client)
        {
            var c = ClientsSocket.First(p => p.ClientSocket == client);
            if (c != null)
            {
                ClientsSocket.Remove(c);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
