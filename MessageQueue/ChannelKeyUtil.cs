namespace MessageQueue
{
    public static class ChannelKeyUtil
    {
        public const string PublicOrderbookKey = "PUBLIC." + MessageType.ORDERBOOK;

        public static string PublicTradeKey = "PUBLIC." + MessageType.TRADE;

        public static string GetPrivateOrderKey(string clientSecret)
            => $"PRIVATE.{clientSecret}";

        public static bool IsPublicOrderbookKey(string key) 
            => string.Equals(PublicOrderbookKey, key);

        public static bool IsPublicTradeKey(string key)
            => string.Equals(PublicTradeKey, key);

        public static bool IsPrivateOrderKey(string key)
            => key.StartsWith("PRIVATE");
    }
}
