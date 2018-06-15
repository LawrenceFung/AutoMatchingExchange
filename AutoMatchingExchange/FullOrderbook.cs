using AutoMatchingExchange.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMatchingExchange
{
    public class FullOrderbook : IOrderbook
    {
        private readonly object syncRoot;

        public SortedDictionary<decimal, LimitPriceLevel> BidLevelsMap { get; private set; }
        public SortedDictionary<decimal, LimitPriceLevel> AskLevelsMap { get; private set; }

        public IEnumerable<LimitPriceLevel> BidLevels { get => BidLevelsMap.Values as IEnumerable<LimitPriceLevel>; }
        public IEnumerable<LimitPriceLevel> AskLevels { get => AskLevelsMap.Values as IEnumerable<LimitPriceLevel>; }

        public event EventHandler<TradeEventArgs> OnTradeUpdated;
        public event EventHandler<OrderbookEventArgs> OnOrderbookUpdated;
        public event EventHandler<LimitOrderEventArgs> OnLimitOrderUpdated;

        public LimitPriceLevel BestBidLevel
        {
            get
            {
                var enumerator = BidLevelsMap.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current.Value;
            }
        }
        public LimitPriceLevel BestAskLevel
        {
            get
            {
                var enumerator = AskLevelsMap.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current.Value;
            }
        }
        
        public Dictionary<string, LimitOrder> Orders { get; private set; }

        public FullOrderbook()
        {
            syncRoot = new object();
            BidLevelsMap = new SortedDictionary<decimal, LimitPriceLevel>(new Utils.DescendingComparer<decimal>());
            AskLevelsMap = new SortedDictionary<decimal, LimitPriceLevel>();
            Orders = new Dictionary<string, LimitOrder>();
        }

        private void AddOrder(LimitOrder order)
        {
            var map = (order.IsBid) ? BidLevelsMap : AskLevelsMap;
            LimitPriceLevel limitLevel;
            if (!map.TryGetValue(order.LimitPrice, out limitLevel))
            {
                limitLevel = new LimitPriceLevel(order);
                map.Add(order.LimitPrice, limitLevel);
                Orders.Add(order.OrderId, order);
            }
            else
            {
                limitLevel.AddOrder(order);
                Orders.Add(order.OrderId, order);
            }
            OnLimitOrderUpdated?.Invoke(this, new LimitOrderEventArgs(order));
        }

        private void UpdateOrderWithoutChangePrice(LimitOrder order)
        {
            //update qty only
            var map = (order.IsBid) ? BidLevelsMap : AskLevelsMap;
            LimitPriceLevel limitLevel;
            if (!map.TryGetValue(order.LimitPrice, out limitLevel))
            {
                throw new Exception(string.Format("UpdateOrderWithoutChangePrice: No such Limit Level for price = {0}", order.LimitPrice));
            }
            else
            {
                limitLevel.UpdateOrder(order);
                OnLimitOrderUpdated?.Invoke(this, new LimitOrderEventArgs(order));
            }
        }

        private void UpdateOrderPrice(LimitOrder newOrder, LimitOrder orderToBeUpdated)
        {
            //update price and qty
            RemoveOrderByPriceLevel(orderToBeUpdated);
            AddOrder(newOrder);
        }

        private void AddOrUpdate(LimitOrder order)
        {
            LimitOrder orderToBeUpdated;
            if (!Orders.TryGetValue(order.OrderId, out orderToBeUpdated))
            {
                AddOrder(order);
            }
            else
            {
                if (order.IsBid != orderToBeUpdated.IsBid) return;
                if (order.LimitPrice == orderToBeUpdated.LimitPrice)
                {
                    UpdateOrderWithoutChangePrice(order);
                }
                else
                {
                    UpdateOrderPrice(order, orderToBeUpdated);
                }
            }
        }

        private void MatchOrder(LimitOrder newOrder)
        {
            var leavesQty = newOrder.Quantity;
            List<LimitOrder> orderMatchedAllQtyAndNeedRemoved = new List<LimitOrder>();
            var enumerator = newOrder.IsBid ? AskLevelsMap.GetEnumerator() : BidLevelsMap.GetEnumerator();
            while (enumerator.MoveNext() && leavesQty > 0)
            {
                var levelToBeMatched = enumerator.Current.Value;
                if ((newOrder.IsBid && newOrder.LimitPrice >= levelToBeMatched.LimitPrice)
                    || (!newOrder.IsBid && newOrder.LimitPrice <= levelToBeMatched.LimitPrice))
                {
                    //var orderToBeMatchedNode = levelToBeMatched.Orders.List.First;
                    //while (orderToBeMatchedNode != null)
                    //{
                    //    var matchedPrice = newOrder.LimitPrice; //In favor to maker.
                    //    decimal matchedQty;
                    //    if (leavesQty >= orderToBeMatchedNode.Value.Quantity)
                    //    {
                    //        matchedQty = orderToBeMatchedNode.Value.Quantity;
                    //        orderToBeMatchedNode.Value.SetQuantity(0M);
                    //        orderMatchedAllQtyAndNeedRemoved.Add(orderToBeMatchedNode.Value);
                    //    }
                    //    else
                    //    {
                    //        matchedQty = leavesQty;
                    //        orderToBeMatchedNode.Value.SetQuantity(orderToBeMatchedNode.Value.Quantity - matchedQty);
                    //    }
                    //    leavesQty -= matchedQty;

                    //    //Private message to newOrder owner
                    //    var tempOrderState = newOrder.DeepCopy();
                    //    tempOrderState.SetQuantity(leavesQty);
                    //    OnLimitOrderUpdated?.Invoke(this, new LimitOrderEventArgs(tempOrderState));

                    //    //Private message to orderToBeMatched owner
                    //    OnLimitOrderUpdated?.Invoke(this, new LimitOrderEventArgs(orderToBeMatchedNode.Value));

                    //    //Public channel to all paraticipants
                    //    var trade = new Trade(newOrder.ReceivedTimestamp, matchedPrice, matchedQty, newOrder.IsBid, newOrder.OrderId, orderToBeMatchedNode.Value.OrderId);
                    //    OnTradeUpdated?.Invoke(this, new TradeEventArgs(trade));


                    //    if (leavesQty <= 0) break;
                    //    orderToBeMatchedNode = orderToBeMatchedNode.Next;
                    //}



                    foreach (var orderToBeMatched in levelToBeMatched.Orders.List)
                    {
                        var matchedPrice = newOrder.LimitPrice; //In favor to maker.
                        decimal matchedQty;
                        LimitOrder updatedOrder;
                        if (leavesQty >= orderToBeMatched.Quantity)
                        {
                            matchedQty = orderToBeMatched.Quantity;
                            updatedOrder = orderToBeMatched.DeepCopyWithQtyChanged(0M);
                            levelToBeMatched.UpdateOrder(updatedOrder);
                            orderMatchedAllQtyAndNeedRemoved.Add(orderToBeMatched);
                        }
                        else
                        {
                            matchedQty = leavesQty;
                            updatedOrder = orderToBeMatched.DeepCopyWithQtyChanged(
                                orderToBeMatched.Quantity - matchedQty);
                            levelToBeMatched.UpdateOrder(updatedOrder);
                        }
                        leavesQty -= matchedQty;

                        //Private message to newOrder owner (taker)
                        var tempOrderState = newOrder.DeepCopyWithQtyChanged(leavesQty);
                        OnLimitOrderUpdated?.Invoke(this, new LimitOrderEventArgs(tempOrderState));

                        //Private message to updatedOrder owner (,aker)
                        OnLimitOrderUpdated?.Invoke(this, new LimitOrderEventArgs(updatedOrder));

                        //Public channel to all paraticipants
                        var trade = new Trade(newOrder.ReceivedTimestamp, matchedPrice, matchedQty, newOrder.IsBid, newOrder.OrderId, orderToBeMatched.OrderId);
                        OnTradeUpdated?.Invoke(this, new TradeEventArgs(trade));

                        if (leavesQty <= 0) break;
                    }
                }
                else break;
            }

            foreach (var item in orderMatchedAllQtyAndNeedRemoved) RemoveOrderByOrderId(item);

            if (leavesQty > 0)
            {
                var newOrderWithLeavesQty = new LimitOrder(newOrder.OrderId, newOrder.IsBid, leavesQty, newOrder.LimitPrice, newOrder.ReceivedTimestamp);
                AddOrder(newOrderWithLeavesQty);
            }
        }

        public void PlaceOrder(LimitOrder newOrder)
        {
            lock (syncRoot)
            {
                if (newOrder.IsBid)
                {
                    var bestAskPrice = (BestAskLevel == null) ? decimal.MaxValue : BestAskLevel.LimitPrice;
                    if (newOrder.LimitPrice >= bestAskPrice) MatchOrder(newOrder);
                    else AddOrUpdate(newOrder);
                }
                else
                {
                    var bestBidPrice = (BestBidLevel == null) ? decimal.MinValue : BestBidLevel.LimitPrice;
                    if (newOrder.LimitPrice <= bestBidPrice) MatchOrder(newOrder);
                    else AddOrUpdate(newOrder);
                }
                OnOrderbookUpdated?.Invoke(this, new OrderbookEventArgs(this));
            }
        }

        private void RemoveOrderByPriceLevel(LimitOrder order)
        {
            var map = (order.IsBid) ? BidLevelsMap : AskLevelsMap;
            LimitPriceLevel limitLevel;
            if (!map.TryGetValue(order.LimitPrice, out limitLevel))
            {
                throw new Exception(string.Format("RemoveOrder: No such Limit Level for price = {0}", order.LimitPrice));
            }
            else
            {
                LimitOrder orderRemoved;
                limitLevel.RemoveOrder(order.OrderId, out orderRemoved);
                if (!limitLevel.Orders.Any() && limitLevel.TotalLevelQuantity == 0)
                {
                    map.Remove(limitLevel.LimitPrice);
                }
                Orders.Remove(order.OrderId);
            }
        }

        public void RemoveOrderByOrderId(LimitOrder order)
        {
            lock (syncRoot)
            {
                LimitOrder orderToBeRemoved;
                if (!Orders.TryGetValue(order.OrderId, out orderToBeRemoved))
                {
                    throw new Exception(string.Format("RemoveOrderWithoutPrice: No such Order for OrdefrId = {0}", order.OrderId));
                }
                else
                {
                    RemoveOrderByPriceLevel(orderToBeRemoved);
                }
                OnOrderbookUpdated?.Invoke(this, new OrderbookEventArgs(this));
            }
        }


        public List<LimitPriceLevel> GetBestBidLevels(int level)
        {
            int bidLevel = 1;
            var bestBidLevels = new List<LimitPriceLevel>();
            foreach (var item in BidLevelsMap.Values)
            {
                bestBidLevels.Add(item);
                if (bidLevel++ > level) break;
            }
            return bestBidLevels;
        }

        public List<LimitPriceLevel> GetBestAskLevels(int level)
        {
            int askLevel = 1;
            var bestAskLevels = new List<LimitPriceLevel>();
            foreach (var item in AskLevelsMap.Values)
            {
                bestAskLevels.Add(item);
                if (askLevel++ > level) break;
            }
            return bestAskLevels;
        }


        private string ToLevel2OrderbookString()
        {
            var builder = new StringBuilder();
            int bidLevel = 1;
            foreach (var item in BidLevelsMap.Values)
            {
                builder.AppendLine(string.Format("Bid lv:{0} price:{1} totalQty:{2} totalOrders:{3}",
                    bidLevel++, item.LimitPrice, item.TotalLevelQuantity, item.NumberOfOrders));
            }
            int askLevel = 1;
            foreach (var item in AskLevelsMap.Values)
            {
                builder.AppendLine(string.Format("Ask lv:{0} price:{1} totalQty:{2} totalOrders:{3}",
                    askLevel++, item.LimitPrice, item.TotalLevelQuantity, item.NumberOfOrders));
            }
            if (builder.Length == 0) return "No NonZero Qty Orders";
            else return builder.ToString();
        }

        public override string ToString() => ToLevel2OrderbookString();
    }
}
