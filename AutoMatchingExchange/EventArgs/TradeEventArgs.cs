namespace AutoMatchingExchange.EventArgs
{
    public class TradeEventArgs: System.EventArgs
    {
        public Trade Trade { get; private set; }

        public TradeEventArgs(Trade trade)
        {
            Trade = trade;
        }
    }
}
