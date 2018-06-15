using AutoMatchingExchange.EventArgs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMatchingExchange
{
    public class OrderManager
    {
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object syncRoot = new object();
        private readonly ConcurrentDictionary<string, string> orderIdToClientIdMap = new ConcurrentDictionary<string, string>();
        private readonly OrderIdProvider orderIdProvider = new OrderIdProvider();
        //private readonly HashSet<string> clientIds = new HashSet<string>();
        private readonly ConcurrentDictionary<string, string> clientIdToClientSecretMap = new ConcurrentDictionary<string, string>();

        public IOrderbook Orderbook { get; private set; }

        public event EventHandler<PublicChannelEventArgs> OnPublicChannelOrderbookUpdated;
        public event EventHandler<PublicChannelEventArgs> OnPublicChannelTradeUpdated;
        public event EventHandler<PrivateMessageEventArgs> OnPrivateMessageUpdated;

        public OrderManager()
        {
            this.Orderbook = new FullOrderbook();
            this.Orderbook.OnTradeUpdated += WhenTradeUpdated;
            this.Orderbook.OnOrderbookUpdated += WhenOrderbookUpdated;
            this.Orderbook.OnLimitOrderUpdated += WhenLimitOrderUpdated;
        }

        public string Login(string clientId)
        {
            logger.Debug($"Login received: clientId = {clientId}");
            var clientSecret = clientId.GetHashCode().ToString();
            if (clientIdToClientSecretMap.TryAdd(clientId, clientSecret))
                return clientSecret;
            else return string.Empty;
        }

        private bool LoggedIn(string clientId) => clientIdToClientSecretMap.ContainsKey(clientId);

        public bool PlaceOrder(LimitOrder order, string clientId)
        {
            logger.Debug($"PlaceOrder received: clientId = {clientId}, order = {order}");
            lock (syncRoot)
            {
                try
                {
                    if (!LoggedIn(clientId)) return false;
                    if (order.LimitPrice <= 0) return false;
                    if (order.Quantity <= 0) return false;

                    //No OrderId means this is a new order. OrderId should be assigned by OrderManager only.
                    if (string.IsNullOrEmpty(order.OrderId))
                    {
                        var newOrderId = orderIdProvider.GetNextOrderId();
                        if (orderIdToClientIdMap.TryAdd(newOrderId, clientId))
                        {
                            order.SetOrderId(newOrderId);
                            Orderbook.PlaceOrder(order);
                            return true;
                        }
                        else return false;
                    }
                    else //Update order
                    {
                        if (orderIdToClientIdMap.TryGetValue(order.OrderId, out string clientOrderId))
                        {
                            Orderbook.PlaceOrder(order);
                            return true;
                        }
                        else return false;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return false;
                }
            }
        }

        public bool CancelOrder(string orderId, string clientId)
        {
            logger.Debug($"PlaceOrder received: clientId = {clientId}, orderId = {orderId}");
            lock (syncRoot)
            {
                try
                {
                    if (!LoggedIn(clientId)) return false;
                    if (orderIdToClientIdMap.TryGetValue(orderId, out string clientIdInMap) && clientIdInMap.Equals(clientId))
                    {
                        var order = new LimitOrder();
                        order.SetOrderId(orderId);
                        Orderbook.RemoveOrderByOrderId(order);
                        return true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return false;
                }
            }
        }

        public void WhenTradeUpdated(object sender, TradeEventArgs e)
        {
            OnPublicChannelTradeUpdated?.Invoke(this, new PublicChannelEventArgs(e.Trade.ToString()));
        }

        public void WhenOrderbookUpdated(object sender, OrderbookEventArgs e)
        {
            OnPublicChannelOrderbookUpdated?.Invoke(this, new PublicChannelEventArgs(e.Orderbook.ToString()));
        }

        public void WhenLimitOrderUpdated(object sender, LimitOrderEventArgs e)
        {
            var orderId = e.LimitOrder.OrderId;
            if (orderIdToClientIdMap.TryGetValue(orderId, out string clientId)
                && clientIdToClientSecretMap.TryGetValue(clientId, out string clientSecret))
            {
                var message = e.LimitOrder.ToString();
                OnPrivateMessageUpdated?.Invoke(this, new PrivateMessageEventArgs(clientSecret, message));
            }
        }

        private class OrderIdProvider
        {
            private long latestOrderId = 0L;
            public string GetNextOrderId() => (latestOrderId++).ToString();
        }
    }
}
