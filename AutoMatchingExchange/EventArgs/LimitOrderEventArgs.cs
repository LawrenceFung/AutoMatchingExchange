namespace AutoMatchingExchange.EventArgs
{
    public class LimitOrderEventArgs: System.EventArgs
    {
        public LimitOrder LimitOrder { get; private set; }

        public LimitOrderEventArgs(LimitOrder limitOrder)
        {
            this.LimitOrder = limitOrder;
        }
    }
}
