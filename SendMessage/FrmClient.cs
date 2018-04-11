using Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMessage
{
    public partial class FrmClient : Form
    {
        private MsgClient client;

        public FrmClient()
        {
            InitializeComponent();
        }

        private void FrmClient_Load(object sender, EventArgs e)
        {
            txtUserName.Text = Guid.NewGuid().ToString();
            btnSend.Enabled = false;
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConnection_Click(object sender, EventArgs e)
        {
            btnConnection.Enabled = false;
            AddMsg("正在连接服务器...");
            var s = await Task.Run(() => { return Connection(); });
            if (s)
            {
                AddMsg("成功连接服务器");
            }
            else
            {
                AddMsg("连接服务器失败");
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSend_Click(object sender, EventArgs e)
        {
            var msg = new MessageEntity();
            msg.MsgStr = rtxtSend.Text;
            msg.MsgType = MessageType.Text;
            msg.FromUser = txtUserName.Text;
            msg.ToUser = cmbUsers.Text;

            var s = await Task.Run(() => { return MessageHelper.SendMessage(msg, client.ClientSocket); });
            //var s = MessageHelper.SendMessage(msg, client.ClientSocket);
            if (!s)
            {
                AddMsg("发送失败！");
            }
        }



        /// <summary>
        /// 连接服务器，并进行登录
        /// </summary>
        /// <returns></returns>
        private bool Connection()
        {
            try
            {
                //连接服务器
                client = new MsgClient(7000, "127.0.0.1");
                client.Connect();
                AddMsg("连接成功，正在发送登录请求...");
                //接收服务器反馈
                //client.DoServerHandler += Receive;
                //Task.Run(() => { client.Receive(); });
                Task.Run(() => Receive(client.ClientSocket));
                //登录
                return LoginIn();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 登录服务器
        /// </summary>
        /// <returns></returns>
        private bool LoginIn()
        {
            var msg = new MessageEntity();
            msg.MsgType = MessageType.LoginIn;
            msg.FromUser = txtUserName.Text;

            var s =  MessageHelper.SendMessage(msg, client.ClientSocket);
            if (!s)
            {
                AddMsg("发送失败！");
            }
            else
            {
                AddMsg("发送登录请求成功！");
            }

            return s;
        }

        /// <summary>
        /// 接收服务器反馈信息
        /// </summary>
        /// <param name="socket"></param>
        private void Receive(Socket socket)
        {
            try
            {
                while (true)
                {
                    var msg = MessageHelper.ReceiveMessageNew(socket);
                    if (msg == null)
                    {
                        AddMsg("获取信息异常！");
                        break;
                    }
                    else
                    {
                        AddMsg("获取到信息：" + msg.MsgType);
                        ReceiveHandler(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                AddMsg(ex.Message + ex.StackTrace);
                client.ClientSocket.Dispose();
                btnConnection.Enabled = true;
                btnSend.Enabled = false;
            }
        }

        /// <summary>
        /// 处理服务器反馈信息
        /// </summary>
        /// <param name="msg"></param>
        private void ReceiveHandler(MessageEntity msg)
        {
            switch (msg.MsgType)
            {
                case MessageType.Text:
                    ShowMsg(msg);
                    return;
                case MessageType.GetUser:
                    cmbUsers.Items.Clear();
                    cmbUsers.Items.AddRange(msg.MsgStr.Split(','));
                    cmbUsers.Items.Insert(0, "");
                    return;
                case MessageType.LoginIn:
                    var s = bool.Parse(msg.MsgStr);
                    btnSend.Enabled = s;
                    btnConnection.Enabled = !s;
                    System.Threading.Timer showImageTimer = new System.Threading.Timer(new TimerCallback(GetUser), null, Timeout.Infinite, 3000);
                    showImageTimer.Change(0, 10000);
                    return;
                default: return;
            }
        }

        #region 处理方式

        /// <summary>
        /// 获取其他用户信息
        /// </summary>
        private void GetUser(object sender)
        {
            try
            {
                var msg = new MessageEntity();
                msg.MsgType = MessageType.GetUser;

                var s = MessageHelper.SendMessage(msg, client.ClientSocket);
                if (!s)
                {
                    AddMsg("获取用户列表失败！");
                }
            }
            catch (Exception ex)
            {
                AddMsg("获取用户列表失败！");
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMsg(MessageEntity msg)
        {
            AddMsg(msg.FromUser + "：\r\n" + msg.MsgStr);
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="msg"></param>
        private void AddMsg(string msg)
        {
            Invoke(new Action(()=> 
            {
                rtxtReceive.AppendText(string.Format("【{0}】{1}\r\n", DateTime.Now, msg));
            }));
        }

        #endregion 处理方式
    }
}
