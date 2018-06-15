using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace MessageQueue
{
    public class ZmqRequester
    {
        private string endpoint;
        private TimeSpan receiveTimeout, pollTimeout;
        private readonly ZSocket socket;
        private readonly ZPollItem poll;
        private Action<Exception> errorReceived;
        private bool isSet;
        private bool isStart;

        public ZmqRequester(ZmqContext context)
        {
            socket = new ZSocket(context.Context, ZSocketType.REQ);
            poll = ZPollItem.CreateReceiver();
            isSet = false;
            isStart = false;
        }

        public void Set(string endpoint, int receiveTimeout, int pollTimeout)
        {
            if (!isSet)
            {
                this.endpoint = endpoint;
                this.receiveTimeout = TimeSpan.FromMilliseconds(receiveTimeout);
                this.pollTimeout = TimeSpan.FromMilliseconds(pollTimeout);
                isSet = true;
            }
        }

        public void Start(Action<Exception> errorReceived)
        {
            if (isSet && !isStart)
            {
                this.errorReceived = errorReceived;
                socket.Connect(endpoint);
                socket.ReceiveTimeout = receiveTimeout;
                isStart = true;
            }
        }

        public void Stop()
        {
            if (isStart)
                socket.Disconnect(endpoint);
        }


        public ZMessage Login(string clientId)
        {
            if (!isStart) return null;
            socket.Send(new ZMessage(new List<ZFrame>
            {
                new ZFrame(MessageType.LOGIN),
                new ZFrame(clientId),
            }));
            return OnWaitingResponse();
        }

        public ZMessage PlaceOrder(string orderId, bool isBid, decimal qty, decimal price, DateTimeOffset timestamp, string clientId)
        {
            if (!isStart) return null;
            socket.Send(new ZMessage(new List<ZFrame>
            {
                new ZFrame(MessageType.PLACEORDER),
                new ZFrame(orderId),
                new ZFrame(isBid.ToString()),
                new ZFrame(qty.ToString()),
                new ZFrame(price.ToString()),
                new ZFrame(timestamp.ToUnixTimeMilliseconds()),
                new ZFrame(clientId),
            }));
            return OnWaitingResponse();
        }

        public ZMessage CancelOrder(string orderId, string clientId)
        {
            if (!isStart) return null;
            socket.Send(new ZMessage(new List<ZFrame>
            {
                new ZFrame(MessageType.CANCELORDER),
                new ZFrame(orderId),
                new ZFrame(clientId),
            }));
            return OnWaitingResponse();
        }

        private ZMessage OnWaitingResponse()
        {
            while (true)
            {
                if (socket.PollIn(poll, out ZMessage message, out ZError error, pollTimeout))
                {
                    if (error != null)
                    {
                        errorReceived(new ZException(error));
                        return message;
                    }
                    return message;
                }
            }
        }
    }
}
