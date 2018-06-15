namespace AutoMatchingExchange.EventArgs
{
    public class OrderbookEventArgs: System.EventArgs
    {
        public IOrderbook Orderbook { get; private set; }

        public OrderbookEventArgs(IOrderbook orderbook)
        {
            Orderbook = orderbook;
        }
    }
}
