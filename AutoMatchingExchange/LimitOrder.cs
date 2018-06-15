using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMatchingExchange
{
    public class LimitOrder
    {
        public string OrderId { get; private set; }  //key
        public bool IsBid { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal LimitPrice { get; private set; }
        public DateTimeOffset ReceivedTimestamp { get; private set; }

        [JsonIgnore]
        public LimitPriceLevel ParentLevel { get; private set; }

        public LimitOrder() { }

        public LimitOrder(string orderId, bool isBid, decimal quantity, decimal price, DateTimeOffset receivedUpdateTs)
        {
            this.OrderId = orderId;
            this.IsBid = isBid;
            this.Quantity = quantity;
            this.LimitPrice = price;
            this.ReceivedTimestamp = receivedUpdateTs;
        }

        public void SetOrderId(string orderId)
        {
            if (!string.IsNullOrEmpty(this.OrderId)) throw new Exception($"Already has orderId = {orderId}");
            this.OrderId = orderId;
        }

        public void SetParentLevel(LimitPriceLevel level)
        {
            ParentLevel = level;
        }

        public override string ToString()
         => JsonConvert.SerializeObject(this, Formatting.None);

        public LimitOrder DeepCopyWithQtyChanged(decimal newQty)
            => new LimitOrder(
                this.OrderId,
                this.IsBid,
                newQty,
                this.LimitPrice,
                this.ReceivedTimestamp);
    }
}
