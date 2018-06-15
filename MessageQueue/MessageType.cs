using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue
{
    public static class MessageType
    {
        #region Requester
        public const int LOGIN = 1001;
        public const int PLACEORDER = 1011;
        public const int CANCELORDER = 1021;
        #endregion

        #region Replyer
        public const int LOGIN_REPLY = 2001;
        public const int PLACEORDER_REPLY = 2011;
        public const int CANCELORDER_REPLY = 2021;
        #endregion

        #region Publisher
        public const string ORDERBOOK = "OB";
        public const string TRADE = "TRADE";
        #endregion
    }
}
