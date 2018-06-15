using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMatchingExchange
{
    public class LimitPriceLevel
    {
        public decimal LimitPrice { get; private set; }  //key
        public decimal TotalLevelQuantity { get; private set; }
        public int NumberOfOrders { get; private set; }
        public bool IsBid { get; private set; }

        public LimitOrdersLinkedList Orders { get; private set; }


        public LimitPriceLevel(LimitOrder order)
        {
            LimitPrice = order.LimitPrice;
            TotalLevelQuantity = order.Quantity;
            NumberOfOrders = 1;
            IsBid = order.IsBid;
            Orders = new LimitOrdersLinkedList();
            Orders.AddFirst(order);
            order.SetParentLevel(this);
        }


        public void AddOrder(LimitOrder order)
        {
            TotalLevelQuantity += order.Quantity;
            NumberOfOrders += 1;
            Orders.AddLast(order);
            order.SetParentLevel(this);
        }


        public bool RemoveOrder(string orderId, out LimitOrder order)
        {
            var success = Orders.Remove(orderId, out order);
            if (success)
            {
                TotalLevelQuantity -= order.Quantity;
                NumberOfOrders -= 1;
            }
            else
            {
                order = null;
            }
            return success;
        }

        public bool UpdateOrder(LimitOrder order)
        {
            decimal prevQty = 0;
            var success = Orders.Update(order, out prevQty);
            if (success)
            {
                TotalLevelQuantity = TotalLevelQuantity - prevQty + order.Quantity;
            }
            return success;
        }
    }
}
