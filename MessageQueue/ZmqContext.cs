using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace MessageQueue
{
    public class ZmqContext : IDisposable
    {
        public ZContext Context { get; private set; }

        public ZmqContext()
        {
            Context = new ZContext();
        }

        public void Stop()
        {
            Context.Shutdown();
            Context.Terminate();
        }

        public void Dispose()
        {
            if (Context != null) Context.Dispose();
        }
    }
}
