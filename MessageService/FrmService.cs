using Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageService
{
    public partial class FrmService : Form
    {
        private MsgService server;

        public FrmService()
        {
            InitializeComponent();
        }

        private void FrmService_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 开始侦听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnListen_Click(object sender, EventArgs e)
        {
            AddMsg("正在创建服务器...");
            server = new MsgService(10, 7000);
            server.DoClientHandler += Receive;
            AddMsg("建服务器创建成功！");

            Task.Run(()=> { server.Receive(); });
            AddMsg("服务器开始侦听...");
        }



        /// <summary>
        /// 接收并处理客户端的请求
        /// </summary>
        /// <param name="client"></param>
        private void Receive(ClientEntity client)
        {
            try
            {
                while (server.IsListen)
                {
                    //var msg = MessageHelper.ReceiveMessage(socket);
                    var msg = MessageHelper.ReceiveMessageNew(client.ClientSocket);
                    if (msg == null)
                    {
                        AddMsg("获取信息异常！");
                    }
                    else
                    {
                        AddMsg(string.Format("获取到来自{0}的请求：{1}", msg.FromUser, msg.MsgType));
                        ReceiveHandler(msg, client);
                    }
                }
            }
            catch (Exception ex)
            {
                AddMsg(ex.Message + ex.StackTrace);
                //退出
                server.LogOut(client.ClientSocket);
            }
        }

        /// <summary>
        /// 处理客户端的请求
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        private void ReceiveHandler(MessageEntity msg, ClientEntity client)
        {
            switch (msg.MsgType)
            {
                case MessageType.Text:
                    ShowMsg(msg);
                    return;
                case MessageType.GetUser:
                    SenUserList(client);
                    return;
                case MessageType.LoginIn:
                    LoginIn(msg, client);
                    return;
                default: return;
            }
        }

        #region 处理方式

        /// <summary>
        /// 发送用户列表
        /// </summary>
        /// <param name="client"></param>
        private void SenUserList(ClientEntity client)
        {
            var smsg = new MessageEntity();
            smsg.MsgType = MessageType.GetUser;
            smsg.MsgStr = string.Join(",", server.ClientsSocket.Select(p => p.UserName).ToArray());
            MessageHelper.SendMessage(smsg, client.ClientSocket);
            AddMsg("发送用户列表：" + client.UserName);
        }

        /// <summary>
        /// 客户端登录操作
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        private void LoginIn(MessageEntity msg, ClientEntity client)
        {
            var s = false;
            if (!server.ClientsSocket.Any(p => p.UserName == msg.FromUser))
            {
                client.UserName = msg.FromUser;
                client.IP = msg.From;
                s = true;
            }

            //发送登录结果
            var smsg = new MessageEntity();
            smsg.MsgType = MessageType.LoginIn;
            smsg.MsgStr = s.ToString();
            MessageHelper.SendMessage(smsg, client.ClientSocket);
            AddMsg(string.Format("发送给{0}登录请求结果：{1}", msg.FromUser, s));
        }

        /// <summary>
        /// 转发客户端的信息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMsg(MessageEntity msg)
        {
            if (string.IsNullOrWhiteSpace(msg.ToUser))
            {
                AddMsg(msg.FromUser + "：\r\n" + msg.MsgStr);
            }
            else
            {
                var c = server.ClientsSocket.First(p => p.UserName == msg.ToUser);
                if (c != null)
                {
                    MessageHelper.SendMessage(msg, c.ClientSocket);
                    AddMsg(string.Format("从{0}转发信息给{1}", msg.FromUser, c.UserName));
                }
            }
        }

        /// <summary>
        /// 显示客户端的信息
        /// </summary>
        /// <param name="msg"></param>
        private void AddMsg(string msg)
        {
            Invoke(new Action(()=> 
            {
                rtxtMsg.AppendText(string.Format("【{0}】{1}\r\n", DateTime.Now, msg));
            }));
        }

        #endregion 处理方式
    }
}
