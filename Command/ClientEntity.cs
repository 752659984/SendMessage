using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public class ClientEntity
    {
        public Socket ClientSocket { get; set; }

        public string UserName { get; set; }

        public string IP { get; set; }

        public DateTime LastActiveTime { get; set; }
    }
}
