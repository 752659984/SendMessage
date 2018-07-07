using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public enum MessageType
    {
        Text = 0,
        GetUser = 1,
        LoginIn = 2
    }

    public class MessageEntity
    {
        public MessageType MsgType { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string FromUser { get; set; }

        public string ToUser { get; set; }

        public string MsgStr { get; set; }
    }
}
