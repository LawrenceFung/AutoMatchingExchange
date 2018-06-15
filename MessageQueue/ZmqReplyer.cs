using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace MessageQueue
{
    public class ZmqReplyer
    {
        private readonly ZSocket socket;

        private readonly string endpoint;
        private readonly int timeOutMilliSecond;

        public ZmqReplyer(ZmqContext context, string endpoint, int timeOutMilliSecond)
        {
            socket = new ZSocket(context.Context, ZSocketType.REP);
            this.endpoint = endpoint;
            this.timeOutMilliSecond = timeOutMilliSecond;
        }

        public void Start(
            Action<Exception> errorReceived, 
            Func<string, string> loginReceived, 
            Func<string, bool, decimal, decimal, DateTimeOffset, string, bool> placeOrderReceived,
            Func<string, string, bool> cancelOrderRevceived)
        {
            socket.Bind(endpoint);
            var poll = ZPollItem.CreateReceiver();
            while (true)
            {
                if (socket.PollIn(poll, out ZMessage message, out ZError error, TimeSpan.FromMilliseconds(timeOutMilliSecond)))
                {
                    if (error != null)
                    {
                        errorReceived(new ZException(error));
                    }

                    var messageType = message[0].ReadInt32();
                    switch (messageType)
                    {
                        case MessageType.LOGIN:
                            {
                                var clientId = message[1].ReadString();
                                var result = loginReceived(clientId);
                                using (var echo = new ZMessage
                                {
                                    new ZFrame(MessageType.LOGIN_REPLY),
                                    new ZFrame(result)
                                })
                                {
                                    socket.Send(echo);
                                }
                            }
                            break;
                        case MessageType.PLACEORDER:
                            {
                                var orderId = message[1].ReadString();
                                var isBid = bool.Parse(message[2].ReadString());
                                var qty = decimal.Parse(message[3].ReadString());
                                var price = decimal.Parse(message[4].ReadString());
                                var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(message[5].ReadInt64());
                                var clientId = message[6].ReadString();
                                var result = placeOrderReceived(orderId, isBid, qty, price, timestamp, clientId);
                                using (var echo = new ZMessage
                                {
                                    new ZFrame(MessageType.PLACEORDER_REPLY),
                                    new ZFrame(result.ToString())
                                })
                                {
                                    socket.Send(echo);
                                }
                            }
                            break;
                        case MessageType.CANCELORDER:
                            {
                                var orderId = message[1].ReadString();
                                var clientId = message[2].ReadString();
                                var result = cancelOrderRevceived(orderId, clientId);
                                using (var echo = new ZMessage
                                {
                                    new ZFrame(MessageType.CANCELORDER_REPLY),
                                    new ZFrame(result.ToString())
                                })
                                {
                                    socket.Send(echo);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void Stop()
        {
            socket.Unbind(endpoint);
            socket.Close();
        }
    }
}
