using AutoMatchingExchange;
using MessageQueue;
using static MessageQueue.ChannelKeyUtil;
using System.Threading.Tasks;

namespace ExchangeServer
{
    public class Server
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly OrderManager orderManager = new OrderManager();
        private readonly ZmqContext context = new ZmqContext();
        private readonly ZmqReplyer replyer;
        private readonly ZmqPublisher publisher;

        public Server(string serverEndpoint, int timeOutMilliSecond, string channelEndpoint)
        {
            replyer = new ZmqReplyer(context, serverEndpoint, timeOutMilliSecond);
            publisher = new ZmqPublisher(context, channelEndpoint);
            orderManager.OnPublicChannelOrderbookUpdated += (s, e) =>
                publisher.Publish(PublicOrderbookKey, e.Message);
            orderManager.OnPublicChannelTradeUpdated += (s, e) =>
                publisher.Publish(PublicTradeKey, e.Message);
            orderManager.OnPrivateMessageUpdated += (s, e) =>
                publisher.Publish(GetPrivateOrderKey(e.ClientSecret), e.Message);
        }

        public void Start()
        {
            publisher.Start();

            Task.Run(() => 
                replyer.Start(
                    (ex) => logger.Error(ex),
                    orderManager.Login,
                    (orderId, isBid, qty, price, timestamp, clientId) => orderManager.PlaceOrder(new LimitOrder(orderId, isBid, qty, price, timestamp), clientId),
                    orderManager.CancelOrder)
            );
        }
        public void Stop()
        {
            replyer.Stop();
            publisher.Stop();
        }

    }
}
