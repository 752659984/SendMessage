using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Command
{
    public class MessageHelper
    {
        public static string ip = GetSystemInfo.GetClientLocalIPv4Address();

        public static byte[] bufSign = Encoding.UTF8.GetBytes("yideMsg");

        public static bool SendMessage(MessageEntity msg, Socket socket)
        {
            try
            {
                msg.From = ip;
                msg.To = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                
                var bufMsg = Encoding.UTF8.GetBytes(json);
                var bufLen = BitConverter.GetBytes(bufMsg.Length);

                var bufSend = bufLen.Concat(bufMsg).ToArray();
                bufSend = bufSign.Concat(bufSend).ToArray();

                ////模拟粘包
                //bufSend = bufSend.Concat(bufSend).ToArray();

                var send = 0;
                while (send != bufSend.Length)
                {
                    send += socket.Send(bufSend, send, bufSend.Length - send, SocketFlags.None);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static MessageEntity ReceiveMessage(Socket socket)
        {
            try
            {
                //单次接收最大长度
                var buffter = new byte[1024];
                //用IEnumerable，使用Concat时会有问题
                //IEnumerable<byte> bufJson = new byte[0];
                var bufJson = new byte[0];

                //真正需要的数据的长度，实际为 标头+长度+消息数据
                var dataLen = 1;
                //已获取的消息数据长度
                var read = 0;
                //头部（标头+长度）
                var head = bufSign.Length + 4;

                while (read < dataLen)
                {
                    //第一次接收
                    if (read == 0)
                    {
                        //验证标头
                        //var n = socket.Receive(buffter);
                        var n = socket.Receive(buffter, head + 1, SocketFlags.None);
                        if (n <= head)
                        {
                            return null;
                        }
                        var sign = buffter.Take(bufSign.Length);
                        if (!EqualsArray(sign, bufSign))
                        {
                            return null;
                        }

                        //长度   .Take(4)可不用
                        dataLen = BitConverter.ToInt32(buffter.Skip(bufSign.Length).Take(4).ToArray(), 0);

                        //真正要获取的数据
                        //n = dataLen > buffter.Length - head ? buffter.Length - head : dataLen;
                        //bufJson = bufJson.Concat(buffter.Skip(head).Take(n)).ToArray();
                        bufJson = bufJson.Concat(buffter.Skip(head).Take(1)).ToArray();
                        read = bufJson.Count();
                    }
                    else
                    {
                        //第一次之后接收
                        var n = dataLen - read;
                        n = n >= buffter.Length ? buffter.Length : n;

                        var r = socket.Receive(buffter, n, SocketFlags.None);
                        read += r;
                        if (r < buffter.Length)
                        {
                            bufJson = bufJson.Concat(buffter.Take(r)).ToArray();
                        }
                        else
                        {
                            bufJson = bufJson.Concat(buffter).ToArray();
                        }
                    }
                }

                var json = Encoding.UTF8.GetString(bufJson.ToArray());

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageEntity>(json);
                result.From = ip;
                result.To = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static MessageEntity ReceiveMessageNew(Socket socket)
        {
            //单次接收最大长度
            var buffter = new byte[1024];
            var bufJson = new byte[0];

            //真正需要的数据的长度，实际为 标头+长度+消息数据
            var dataLen = 1;
            //已获取的消息数据长度
            var read = -1;
            //头部（标头+长度）
            var head = bufSign.Length + 4;

            while (read < dataLen)
            {
                //第一次接收
                if (read == -1)
                {
                    //验证标头
                    var n = socket.Receive(buffter, head, SocketFlags.None);
                    if (n != head)
                    {
                        return null;
                    }
                    var sign = buffter.Take(bufSign.Length);
                    if (!EqualsArray(sign, bufSign))
                    {
                        return null;
                    }

                    //长度   .Take(4)可不用
                    dataLen = BitConverter.ToInt32(buffter.Skip(bufSign.Length).Take(4).ToArray(), 0);
                    read = 0;
                }
                else
                {
                    //第一次之后接收
                    var n = dataLen - read;
                    n = n >= buffter.Length ? buffter.Length : n;

                    var r = socket.Receive(buffter, n, SocketFlags.None);
                    read += r;
                    if (r < buffter.Length)
                    {
                        bufJson = bufJson.Concat(buffter.Take(r)).ToArray();
                    }
                    else
                    {
                        bufJson = bufJson.Concat(buffter).ToArray();
                    }
                }
            }

            try
            {
                var json = Encoding.UTF8.GetString(bufJson.ToArray());

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageEntity>(json);
                result.From = ip;
                result.To = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool EqualsArray<T>(IEnumerable<T> array1, IEnumerable<T> array2)
        {
            if (array1.Count() == array2.Count() && !array1.Except(array2).Any() && !array2.Except(array1).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
