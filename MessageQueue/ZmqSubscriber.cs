using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using static MessageQueue.ChannelKeyUtil;

namespace MessageQueue
{
    public class ZmqSubscriber : IDisposable
    {
        private readonly ZSocket socket;
        private readonly Thread workerThread;
        private string endpoint;
        private bool isStarted;
        private Action<ZException> errorReceived;

        public event EventHandler<string> OnPublicOrderbookUpdated;
        public event EventHandler<string> OnPublicTradeUpdated;
        public event EventHandler<string> OnPrivateMessageUpdated;

        public ZmqSubscriber(ZmqContext zeroMQContext)
        {
            socket = new ZSocket(zeroMQContext.Context, ZSocketType.SUB);
            socket.Linger = TimeSpan.Zero;
            workerThread = new Thread(HandleMessage);
            isStarted = false;
        }

        ~ZmqSubscriber()
        {
            Dispose(false);
        }

        public void Setup(string endpoint, Action<Exception> errorReceived)
        {
            if (!isStarted)
            {
                this.endpoint = endpoint;
                this.errorReceived = errorReceived;
            }
        }

        public void Start()
        {
            if (!isStarted)
            {
                isStarted = true;

                socket.Connect(endpoint);
                workerThread.Start();
            }
        }

        public void Stop()
        {
            if (isStarted)
            {
                isStarted = false;
                socket.UnsubscribeAll();
                socket.Close();
            }
        }

        private void HandleMessage()
        {
            while (true)
            {
                using (ZMessage message = socket.ReceiveMessage(out ZError error))
                {
                    if (error != null)
                    {
                        errorReceived(new ZException(error));
                    }
                    else
                    {
                        if (message != null)
                            ProcessMessage(message);
                    }
                }
            }
        }

        private void ProcessMessage(ZMessage message)
        {
            var key = message[0].ReadString();
            var data = message[1].ReadString();

            if (IsPublicOrderbookKey(key))
                OnPublicOrderbookUpdated?.Invoke(this, data);
            else if (IsPublicTradeKey(key))
                OnPublicTradeUpdated?.Invoke(this, data);
            else if (IsPrivateOrderKey(key))
                OnPrivateMessageUpdated?.Invoke(this, data);
        }

        public void Subscribe(string key)
        {
            if (isStarted)
                socket.Subscribe(key);
        }

        public void Unsubscribe(string key)
        {
            if (isStarted)
                socket.Unsubscribe(key);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (socket != null)
                {
                    socket.Dispose();
                }
            }
        }
    }
}
