using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeClient.ViewModels
{
    public class PlaceOrderViewModel
    { 

        private string orderId;
        [Category("PlaceOrder")]
        [DisplayName("Order ID")]
        [Description("Only used when Update/Cancel order.")]
        public string OrderId
        {
            get => orderId;
            set => orderId = value;
        }

        private SideOptions side;
        [Category("PlaceOrder")]
        [DisplayName("Side")]
        public SideOptions Side
        {
            get => side;
            set => side = value;
        }

        private decimal qty;
        [Category("PlaceOrder")]
        [DisplayName("Quantity")]
        public decimal Qty
        {
            get => qty;
            set => qty = value;
        }

        private decimal price;
        [Category("PlaceOrder")]
        [DisplayName("Limit Price")]
        public decimal Price
        {
            get => price;
            set => price = value;
        }
    }

    public enum SideOptions
    {
        Buy = 1,
        Sell = 2,
    }
}
