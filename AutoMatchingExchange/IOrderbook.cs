using AutoMatchingExchange.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMatchingExchange
{
    public interface IOrderbook
    {
        LimitPriceLevel BestBidLevel { get; }
        LimitPriceLevel BestAskLevel { get; }

        IEnumerable<LimitPriceLevel> BidLevels { get; }
        IEnumerable<LimitPriceLevel> AskLevels { get; }

        event EventHandler<TradeEventArgs> OnTradeUpdated;
        event EventHandler<OrderbookEventArgs> OnOrderbookUpdated;
        event EventHandler<LimitOrderEventArgs> OnLimitOrderUpdated;

        void PlaceOrder(LimitOrder newOrder);
        void RemoveOrderByOrderId(LimitOrder order);

        List<LimitPriceLevel> GetBestBidLevels(int level);
        List<LimitPriceLevel> GetBestAskLevels(int level);
    }
}
