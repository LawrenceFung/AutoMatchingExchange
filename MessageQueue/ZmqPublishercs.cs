using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace MessageQueue
{
    public class ZmqPublisher : IDisposable
    {
        private readonly ZSocket socket;
        private readonly string endpoint;
        private bool isStart;
        private readonly object syncRoot = new object ();

        public ZmqPublisher(ZmqContext context, string endpoint)
        {
            socket = new ZSocket(context.Context, ZSocketType.PUB);
            socket.Linger = TimeSpan.Zero;
            this.endpoint = endpoint;
            isStart = false;
        }

        public void Start()
        {
            if (!isStart)
            {
                isStart = true;
                socket.Bind(endpoint);
            }
        }

        public void Stop()
        {
            if (isStart)
            {
                isStart = false;
                socket.Unbind(endpoint);
                socket.Close();
            }
        }

        public void Publish(string key, string data)
        {
            lock (syncRoot)
            {
                if (isStart)
                {
                    using (var message = new ZMessage())
                    {
                        message.Add(new ZFrame(key));
                        message.Add(new ZFrame(data));
                        socket.Send(message);
                    }
                }
            }
        }

        public void Dispose()
        {
            if (socket != null)
                socket.Dispose();
        }
    }
}
