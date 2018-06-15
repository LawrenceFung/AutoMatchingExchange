namespace AutoMatchingExchange.EventArgs
{
    public class PublicChannelEventArgs: System.EventArgs
    {
        public string Message { get; private set; }

        public PublicChannelEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
