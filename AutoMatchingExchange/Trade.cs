using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMatchingExchange
{
    public class Trade
    {
        public DateTimeOffset ReceivedTimestamp { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public bool IsTakerBuyer { get; set; }
        public string TakerOrderId { get; set; }
        public string MakerOrderId { get; set; }

        public Trade(DateTimeOffset receivedTimestamp, decimal price, decimal qty, bool isTakerBuyer, string takerOrderId, string makerOrderId)
        {
            ReceivedTimestamp = receivedTimestamp;
            Price = price;
            Quantity = qty;
            IsTakerBuyer = isTakerBuyer;

            TakerOrderId = takerOrderId;
            MakerOrderId = makerOrderId;
        }

        public override string ToString()
            => JsonConvert.SerializeObject(this, Formatting.None);
    }
}
