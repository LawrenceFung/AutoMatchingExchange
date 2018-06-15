namespace AutoMatchingExchange.EventArgs
{
    public class PrivateMessageEventArgs : System.EventArgs
    {
        public string ClientSecret { get; private set; }

        public string Message { get; private set; }

        public PrivateMessageEventArgs(string clientSecret, string message)
        {
            this.ClientSecret = clientSecret;
            this.Message = message;
        }
    }
}
