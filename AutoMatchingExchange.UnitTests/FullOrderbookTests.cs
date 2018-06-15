using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMatchingExchange.UnitTests
{
    [TestClass]
    public class FullOrderbookTests
    {
        private static IOrderbook orderbook;


        [TestMethod]
        public void Place_Single_New_Buy_Order()
        {
            orderbook = new FullOrderbook();

            var orderId = "order1";
            var isBid = true;
            var quantity = 10;
            var price = 100;
            var timestamp = DateTimeOffset.Now;
            var order = new LimitOrder(
                orderId,
                isBid, 
                quantity,
                price,
                timestamp);

            orderbook.PlaceOrder(order);

            Assert.IsNotNull(orderbook.BidLevels);
            Assert.IsNotNull(orderbook.BestBidLevel);
            Assert.AreEqual(quantity, orderbook.BestBidLevel.TotalLevelQuantity);
            Assert.AreEqual(price, orderbook.BestBidLevel.LimitPrice);
            Assert.AreEqual(isBid, orderbook.BestBidLevel.IsBid);
            Assert.AreEqual(1, orderbook.BestBidLevel.NumberOfOrders);
            Assert.IsNotNull(orderbook.BestBidLevel.Orders);
            Assert.IsNotNull(orderbook.BestBidLevel.Orders.List);
            Assert.AreEqual(1, orderbook.BestBidLevel.Orders.List.Count);
            Assert.AreEqual(orderId, orderbook.BestBidLevel.Orders.List.First.Value.OrderId);
            Assert.AreEqual(isBid, orderbook.BestBidLevel.Orders.List.First.Value.IsBid);
            Assert.AreEqual(quantity, orderbook.BestBidLevel.Orders.List.First.Value.Quantity);
            Assert.AreEqual(price, orderbook.BestBidLevel.Orders.List.First.Value.LimitPrice);
            Assert.AreEqual(timestamp, orderbook.BestBidLevel.Orders.List.First.Value.ReceivedTimestamp);
            Assert.IsNotNull(orderbook.BestBidLevel.Orders.Table);
            Assert.AreEqual(1, orderbook.BestBidLevel.Orders.Table.Count);
            Assert.AreSame(orderbook.BestBidLevel.Orders.Table[orderId], 
                orderbook.BestBidLevel.Orders.List.First);
        }

        [TestMethod]
        public void Place_Single_New_Sell_Order()
        {
            orderbook = new FullOrderbook();

            var orderId = "order1";
            var isBid = false;
            var quantity = 10;
            var price = 100;
            var timestamp = DateTimeOffset.Now;
            var order = new LimitOrder(
                orderId,
                isBid,
                quantity,
                price,
                timestamp);

            orderbook.PlaceOrder(order);

            Assert.IsNotNull(orderbook.AskLevels);
            Assert.IsNotNull(orderbook.BestAskLevel);
            Assert.AreEqual(quantity, orderbook.BestAskLevel.TotalLevelQuantity);
            Assert.AreEqual(price, orderbook.BestAskLevel.LimitPrice);
            Assert.AreEqual(isBid, orderbook.BestAskLevel.IsBid);
            Assert.AreEqual(1, orderbook.BestAskLevel.NumberOfOrders);
            Assert.IsNotNull(orderbook.BestAskLevel.Orders);
            Assert.IsNotNull(orderbook.BestAskLevel.Orders.List);
            Assert.AreEqual(1, orderbook.BestAskLevel.Orders.List.Count);
            Assert.AreEqual(orderId, orderbook.BestAskLevel.Orders.List.First.Value.OrderId);
            Assert.AreEqual(isBid, orderbook.BestAskLevel.Orders.List.First.Value.IsBid);
            Assert.AreEqual(quantity, orderbook.BestAskLevel.Orders.List.First.Value.Quantity);
            Assert.AreEqual(price, orderbook.BestAskLevel.Orders.List.First.Value.LimitPrice);
            Assert.AreEqual(timestamp, orderbook.BestAskLevel.Orders.List.First.Value.ReceivedTimestamp);
            Assert.IsNotNull(orderbook.BestAskLevel.Orders.Table);
            Assert.AreEqual(1, orderbook.BestAskLevel.Orders.Table.Count);
            Assert.AreSame(orderbook.BestAskLevel.Orders.Table[orderId],
                orderbook.BestAskLevel.Orders.List.First);
        }

        [TestMethod]
        public void Place_Mutiple_Buy_Orders_With_Same_Price()
        {
            orderbook = new FullOrderbook();

            var limitPrice = 100;
            var isBid = true;

            var orderId_1 = "order1";
            var isBid_1 = isBid;
            var quantity_1 = 10;
            var price_1 = limitPrice;
            var timestamp_1 = DateTimeOffset.Now;
            var order_1 = new LimitOrder(
                orderId_1,
                isBid_1,
                quantity_1,
                price_1,
                timestamp_1);

            var orderId_2 = "order2";
            var isBid_2 = isBid;
            var quantity_2 = 20;
            var price_2 = limitPrice;
            var timestamp_2 = DateTimeOffset.Now;
            var order_2 = new LimitOrder(
                orderId_2,
                isBid_2,
                quantity_2,
                price_2,
                timestamp_2);

            var orderId_3 = "order3";
            var isBid_3 = isBid;
            var quantity_3 = 30;
            var price_3 = limitPrice;
            var timestamp_3 = DateTimeOffset.Now;
            var order_3 = new LimitOrder(
                orderId_3,
                isBid_3,
                quantity_3,
                price_3,
                timestamp_3);

            orderbook.PlaceOrder(order_1);
            orderbook.PlaceOrder(order_2);
            orderbook.PlaceOrder(order_3);

            Assert.IsNotNull(orderbook.BidLevels);
            Assert.IsNotNull(orderbook.BestBidLevel);
            Assert.AreEqual(quantity_1 + quantity_2 + quantity_3, orderbook.BestBidLevel.TotalLevelQuantity);
            Assert.AreEqual(limitPrice, orderbook.BestBidLevel.LimitPrice);
            Assert.AreEqual(isBid, orderbook.BestBidLevel.IsBid);
            Assert.AreEqual(3, orderbook.BestBidLevel.NumberOfOrders);
            Assert.IsNotNull(orderbook.BestBidLevel.Orders);
            Assert.IsNotNull(orderbook.BestBidLevel.Orders.List);
            Assert.AreEqual(3, orderbook.BestBidLevel.Orders.List.Count);
            Assert.IsTrue(orderbook.BestBidLevel.Orders.List.Contains(order_1));
            Assert.IsTrue(orderbook.BestBidLevel.Orders.List.Contains(order_2));
            Assert.IsTrue(orderbook.BestBidLevel.Orders.List.Contains(order_3));
            Assert.AreEqual(3, orderbook.BestBidLevel.Orders.Table.Count);
        }

        [TestMethod]
        public void Place_Mutiple_Sell_Orders_With_Same_Price()
        {
            orderbook = new FullOrderbook();

            var limitPrice = 100;
            var isBid = false;

            var orderId_1 = "order1";
            var isBid_1 = isBid;
            var quantity_1 = 10;
            var price_1 = limitPrice;
            var timestamp_1 = DateTimeOffset.Now;
            var order_1 = new LimitOrder(
                orderId_1,
                isBid_1,
                quantity_1,
                price_1,
                timestamp_1);

            var orderId_2 = "order2";
            var isBid_2 = isBid;
            var quantity_2 = 20;
            var price_2 = limitPrice;
            var timestamp_2 = DateTimeOffset.Now;
            var order_2 = new LimitOrder(
                orderId_2,
                isBid_2,
                quantity_2,
                price_2,
                timestamp_2);

            var orderId_3 = "order3";
            var isBid_3 = isBid;
            var quantity_3 = 30;
            var price_3 = limitPrice;
            var timestamp_3 = DateTimeOffset.Now;
            var order_3 = new LimitOrder(
                orderId_3,
                isBid_3,
                quantity_3,
                price_3,
                timestamp_3);

            orderbook.PlaceOrder(order_1);
            orderbook.PlaceOrder(order_2);
            orderbook.PlaceOrder(order_3);

            Assert.IsNotNull(orderbook.AskLevels);
            Assert.IsNotNull(orderbook.BestAskLevel);
            Assert.AreEqual(quantity_1 + quantity_2 + quantity_3, orderbook.BestAskLevel.TotalLevelQuantity);
            Assert.AreEqual(limitPrice, orderbook.BestAskLevel.LimitPrice);
            Assert.AreEqual(isBid, orderbook.BestAskLevel.IsBid);
            Assert.AreEqual(3, orderbook.BestAskLevel.NumberOfOrders);
            Assert.IsNotNull(orderbook.BestAskLevel.Orders);
            Assert.IsNotNull(orderbook.BestAskLevel.Orders.List);
            Assert.AreEqual(3, orderbook.BestAskLevel.Orders.List.Count);
            Assert.IsTrue(orderbook.BestAskLevel.Orders.List.Contains(order_1));
            Assert.IsTrue(orderbook.BestAskLevel.Orders.List.Contains(order_2));
            Assert.IsTrue(orderbook.BestAskLevel.Orders.List.Contains(order_3));
            Assert.AreEqual(3, orderbook.BestAskLevel.Orders.Table.Count);
        }

        [TestMethod]
        public void Place_Mutiple_Buy_Orders_With_Different_Prices()
        {
            orderbook = new FullOrderbook();

            var isBid = true;

            var orderId_1 = "order1";
            var isBid_1 = isBid;
            var quantity_1 = 30;
            var price_1 = 300;
            var timestamp_1 = DateTimeOffset.Now;
            var order_1 = new LimitOrder(
                orderId_1,
                isBid_1,
                quantity_1,
                price_1,
                timestamp_1);

            var orderId_2 = "order2";
            var isBid_2 = isBid;
            var quantity_2 = 20;
            var price_2 = 200;
            var timestamp_2 = DateTimeOffset.Now;
            var order_2 = new LimitOrder(
                orderId_2,
                isBid_2,
                quantity_2,
                price_2,
                timestamp_2);

            var orderId_3 = "order3";
            var isBid_3 = isBid;
            var quantity_3 = 10;
            var price_3 = 100;
            var timestamp_3 = DateTimeOffset.Now;
            var order_3 = new LimitOrder(
                orderId_3,
                isBid_3,
                quantity_3,
                price_3,
                timestamp_3);

            List<LimitOrder> testOrders = new List<LimitOrder>()
            {
                order_1, order_2, order_3
            };

            orderbook.PlaceOrder(order_1);
            orderbook.PlaceOrder(order_2);
            orderbook.PlaceOrder(order_3);

            Assert.IsNotNull(orderbook.BidLevels);
            Assert.IsNotNull(orderbook.BestBidLevel);
            var bestBidsFromGetBestBidLevels = orderbook.GetBestBidLevels(3);
            Assert.IsNotNull(bestBidsFromGetBestBidLevels);
            Assert.AreEqual(3, bestBidsFromGetBestBidLevels.Count);
            var bidLevelEnumerator = orderbook.BidLevels.GetEnumerator();
            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(bidLevelEnumerator.MoveNext());
                var bidLevel = bidLevelEnumerator.Current;
                var testOrder = testOrders[i];

                Assert.AreEqual(testOrder.Quantity, bidLevel.TotalLevelQuantity);
                Assert.AreEqual(testOrder.LimitPrice, bidLevel.LimitPrice);
                Assert.AreEqual(1, bidLevel.NumberOfOrders);
                Assert.IsNotNull(bidLevel.Orders);
                Assert.IsNotNull(bidLevel.Orders.List);
                Assert.AreEqual(1, bidLevel.Orders.List.Count);
                Assert.AreEqual(testOrder.OrderId, bidLevel.Orders.List.First.Value.OrderId);
                Assert.AreEqual(isBid, bidLevel.Orders.List.First.Value.IsBid);
                Assert.AreEqual(testOrder.Quantity, bidLevel.Orders.List.First.Value.Quantity);
                Assert.AreEqual(testOrder.LimitPrice, bidLevel.Orders.List.First.Value.LimitPrice);
                Assert.AreEqual(testOrder.ReceivedTimestamp, bidLevel.Orders.List.First.Value.ReceivedTimestamp);
                Assert.IsNotNull(bidLevel.Orders.Table);
                Assert.AreEqual(1, bidLevel.Orders.Table.Count);
                Assert.AreSame(bidLevel.Orders.Table[testOrder.OrderId],
                    bidLevel.Orders.List.First);
                Assert.AreSame(bidLevel, bestBidsFromGetBestBidLevels[i]);
            }

        }

        [TestMethod]
        public void Place_Mutiple_Sell_Orders_With_Different_Prices()
        {
            orderbook = new FullOrderbook();

            var isBid = false;

            var orderId_1 = "order1";
            var isBid_1 = isBid;
            var quantity_1 = 10;
            var price_1 = 100;
            var timestamp_1 = DateTimeOffset.Now;
            var order_1 = new LimitOrder(
                orderId_1,
                isBid_1,
                quantity_1,
                price_1,
                timestamp_1);

            var orderId_2 = "order2";
            var isBid_2 = isBid;
            var quantity_2 = 20;
            var price_2 = 200;
            var timestamp_2 = DateTimeOffset.Now;
            var order_2 = new LimitOrder(
                orderId_2,
                isBid_2,
                quantity_2,
                price_2,
                timestamp_2);

            var orderId_3 = "order3";
            var isBid_3 = isBid;
            var quantity_3 = 30;
            var price_3 = 300;
            var timestamp_3 = DateTimeOffset.Now;
            var order_3 = new LimitOrder(
                orderId_3,
                isBid_3,
                quantity_3,
                price_3,
                timestamp_3);

            List<LimitOrder> testOrders = new List<LimitOrder>()
            {
                order_1, order_2, order_3
            };

            orderbook.PlaceOrder(order_1);
            orderbook.PlaceOrder(order_2);
            orderbook.PlaceOrder(order_3);

            Assert.IsNotNull(orderbook.AskLevels);
            Assert.IsNotNull(orderbook.BestAskLevel);
            var bestAsksFromGetBestAskLevels = orderbook.GetBestAskLevels(3);
            Assert.IsNotNull(bestAsksFromGetBestAskLevels);
            Assert.AreEqual(3, bestAsksFromGetBestAskLevels.Count);
            var askLevelEnumerator = orderbook.AskLevels.GetEnumerator();
            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(askLevelEnumerator.MoveNext());
                var askLevel = askLevelEnumerator.Current;
                var testOrder = testOrders[i];

                Assert.AreEqual(testOrder.Quantity, askLevel.TotalLevelQuantity);
                Assert.AreEqual(testOrder.LimitPrice, askLevel.LimitPrice);
                Assert.AreEqual(1, askLevel.NumberOfOrders);
                Assert.IsNotNull(askLevel.Orders);
                Assert.IsNotNull(askLevel.Orders.List);
                Assert.AreEqual(1, askLevel.Orders.List.Count);
                Assert.AreEqual(testOrder.OrderId, askLevel.Orders.List.First.Value.OrderId);
                Assert.AreEqual(isBid, askLevel.Orders.List.First.Value.IsBid);
                Assert.AreEqual(testOrder.Quantity, askLevel.Orders.List.First.Value.Quantity);
                Assert.AreEqual(testOrder.LimitPrice, askLevel.Orders.List.First.Value.LimitPrice);
                Assert.AreEqual(testOrder.ReceivedTimestamp, askLevel.Orders.List.First.Value.ReceivedTimestamp);
                Assert.IsNotNull(askLevel.Orders.Table);
                Assert.AreEqual(1, askLevel.Orders.Table.Count);
                Assert.AreSame(askLevel.Orders.Table[testOrder.OrderId],
                    askLevel.Orders.List.First);
                Assert.AreSame(askLevel, bestAsksFromGetBestAskLevels[i]);
            }
        }

        [TestMethod]
        public void Place_New_Buy_Order_And_Match_Exist_Sell_Orders()
        {
            orderbook = new FullOrderbook();

            var ask_orderId_1 = "ask_order_1";
            var ask_isBid_1 = false;
            var ask_quantity_1 = 5;
            var ask_price_1 = 10;
            var ask_timestamp_1 = DateTimeOffset.Now;
            var ask_order_1 = new LimitOrder(
                ask_orderId_1,
                ask_isBid_1,
                ask_quantity_1,
                ask_price_1,
                ask_timestamp_1);

            var ask_orderId_2 = "ask_order_2";
            var ask_isBid_2 = false;
            var ask_quantity_2 = 5;
            var ask_price_2 = 11;
            var ask_timestamp_2 = DateTimeOffset.Now;
            var ask_order_2 = new LimitOrder(
                ask_orderId_2,
                ask_isBid_2,
                ask_quantity_2,
                ask_price_2,
                ask_timestamp_2);

            var ask_orderId_3 = "ask_order_3";
            var ask_isBid_3 = false;
            var ask_quantity_3 = 5;
            var ask_price_3 = 11;
            var ask_timestamp_3 = DateTimeOffset.Now;
            var ask_order_3 = new LimitOrder(
                ask_orderId_3,
                ask_isBid_3,
                ask_quantity_3,
                ask_price_3,
                ask_timestamp_3);

            var ask_orderId_4 = "ask_order_4";
            var ask_isBid_4 = false;
            var ask_quantity_4 = 5;
            var ask_price_4 = 12;
            var ask_timestamp_4 = DateTimeOffset.Now;
            var ask_order_4 = new LimitOrder(
                ask_orderId_4,
                ask_isBid_4,
                ask_quantity_4,
                ask_price_4,
                ask_timestamp_4);

            var bid_orderId = "bid_order";
            var bid_isBid = true;
            var bid_quantity = 17;
            var bid_price = 12;
            var bid_timestamp = DateTimeOffset.Now;
            var bid_order = new LimitOrder(
                bid_orderId,
                bid_isBid,
                bid_quantity,
                bid_price,
                bid_timestamp);

            orderbook.PlaceOrder(ask_order_1);
            orderbook.PlaceOrder(ask_order_2);
            orderbook.PlaceOrder(ask_order_3);
            orderbook.PlaceOrder(ask_order_4);
            orderbook.PlaceOrder(bid_order);


            //<1>. Should have only one ask level at $12 left.
            //<2>. Only one order at this level. 
            //<3>. TotalLevelQuantity at this level should be 3.
            Assert.IsNull(orderbook.BestBidLevel);

            //Assert <1>
            Assert.IsNotNull(orderbook.BestAskLevel);
            var bestAskLevels = orderbook.GetBestAskLevels(3);
            Assert.AreEqual(1, bestAskLevels.Count);
            Assert.AreEqual(12, orderbook.BestAskLevel.LimitPrice);

            //Assert <2>
            Assert.AreEqual(1, orderbook.BestAskLevel.NumberOfOrders);

            //Assert <3>
            Assert.AreEqual(3, orderbook.BestAskLevel.TotalLevelQuantity);
        }

        [TestMethod]
        public void Place_New_Sell_Order_And_Match_Exist_Buy_Orders()
        {
            orderbook = new FullOrderbook();

            var bid_orderId_1 = "bid_order_1";
            var bid_isBid_1 = true;
            var bid_quantity_1 = 5;
            var bid_price_1 = 23;
            var bid_timestamp_1 = DateTimeOffset.Now;
            var bid_order_1 = new LimitOrder(
                bid_orderId_1,
                bid_isBid_1,
                bid_quantity_1,
                bid_price_1,
                bid_timestamp_1);

            var bid_orderId_2 = "bid_order_2";
            var bid_isBid_2 = true;
            var bid_quantity_2 = 5;
            var bid_price_2 = 22;
            var bid_timestamp_2 = DateTimeOffset.Now;
            var bid_order_2 = new LimitOrder(
                bid_orderId_2,
                bid_isBid_2,
                bid_quantity_2,
                bid_price_2,
                bid_timestamp_2);

            var bid_orderId_3 = "bid_order_3";
            var bid_isBid_3 = true;
            var bid_quantity_3 = 5;
            var bid_price_3 = 21;
            var bid_timestamp_3 = DateTimeOffset.Now;
            var bid_order_3 = new LimitOrder(
                bid_orderId_3,
                bid_isBid_3,
                bid_quantity_3,
                bid_price_3,
                bid_timestamp_3);

            var bid_orderId_4 = "bid_order_4";
            var bid_isBid_4 = true;
            var bid_quantity_4 = 5;
            var bid_price_4 = 21;
            var bid_timestamp_4 = DateTimeOffset.Now;
            var bid_order_4 = new LimitOrder(
                bid_orderId_4,
                bid_isBid_4,
                bid_quantity_4,
                bid_price_4,
                bid_timestamp_4);

            var bid_orderId_5 = "bid_order_5";
            var bid_isBid_5 = true;
            var bid_quantity_5 = 5;
            var bid_price_5 = 21;
            var bid_timestamp_5 = DateTimeOffset.Now;
            var bid_order_5 = new LimitOrder(
                bid_orderId_5,
                bid_isBid_5,
                bid_quantity_5,
                bid_price_5,
                bid_timestamp_5);

            var ask_orderId = "ask_order";
            var ask_isBid = false;
            var ask_quantity = 16;
            var ask_price = 21;
            var ask_timestamp = DateTimeOffset.Now;
            var ask_order = new LimitOrder(
                ask_orderId,
                ask_isBid,
                ask_quantity,
                ask_price,
                ask_timestamp);

            orderbook.PlaceOrder(bid_order_1);
            orderbook.PlaceOrder(bid_order_2);
            orderbook.PlaceOrder(bid_order_3);
            orderbook.PlaceOrder(bid_order_4);
            orderbook.PlaceOrder(bid_order_5);
            orderbook.PlaceOrder(ask_order);


            //<1>. Should have only one bid level at $21 left.
            //<2>. Two orders at this level, with Quantity 4 and 5. 
            //<3>. TotalLevelQuantity at this level should be 9.
            Assert.IsNull(orderbook.BestAskLevel);

            //Assert <1>
            Assert.IsNotNull(orderbook.BestBidLevel);
            var bestBidLevels = orderbook.GetBestBidLevels(3);
            Assert.AreEqual(1, bestBidLevels.Count);
            Assert.AreEqual(21, orderbook.BestBidLevel.LimitPrice);

            //Assert <2>
            Assert.AreEqual(2, orderbook.BestBidLevel.NumberOfOrders);
            var firstBidOrderNode = orderbook.BestBidLevel.Orders.List.First;
            Assert.IsNotNull(firstBidOrderNode);
            Assert.AreEqual(4, firstBidOrderNode.Value.Quantity);
            var secondBidOrderNode = firstBidOrderNode.Next;
            Assert.IsNotNull(secondBidOrderNode);
            Assert.AreEqual(5, secondBidOrderNode.Value.Quantity);

            //Assert <3>
            Assert.AreEqual(9, orderbook.BestBidLevel.TotalLevelQuantity);
        }

        [TestMethod]
        public void Remove_Order_By_OrderId()
        {
            orderbook = new FullOrderbook();

            var bid_orderId_1 = "bid_order_1";
            var bid_isBid_1 = true;
            var bid_quantity_1 = 5;
            var bid_price_1 = 23;
            var bid_timestamp_1 = DateTimeOffset.Now;
            var bid_order_1 = new LimitOrder(
                bid_orderId_1,
                bid_isBid_1,
                bid_quantity_1,
                bid_price_1,
                bid_timestamp_1);

            var bid_orderId_2 = "bid_order_2";
            var bid_isBid_2 = true;
            var bid_quantity_2 = 5;
            var bid_price_2 = 22;
            var bid_timestamp_2 = DateTimeOffset.Now;
            var bid_order_2 = new LimitOrder(
                bid_orderId_2,
                bid_isBid_2,
                bid_quantity_2,
                bid_price_2,
                bid_timestamp_2);

            var bid_orderId_3 = "bid_order_3";
            var bid_isBid_3 = true;
            var bid_quantity_3 = 5;
            var bid_price_3 = 21;
            var bid_timestamp_3 = DateTimeOffset.Now;
            var bid_order_3 = new LimitOrder(
                bid_orderId_3,
                bid_isBid_3,
                bid_quantity_3,
                bid_price_3,
                bid_timestamp_3);

            var bid_orderId_4 = "bid_order_4";
            var bid_isBid_4 = true;
            var bid_quantity_4 = 5;
            var bid_price_4 = 21;
            var bid_timestamp_4 = DateTimeOffset.Now;
            var bid_order_4 = new LimitOrder(
                bid_orderId_4,
                bid_isBid_4,
                bid_quantity_4,
                bid_price_4,
                bid_timestamp_4);

            var bid_orderId_5 = "bid_order_5";
            var bid_isBid_5 = true;
            var bid_quantity_5 = 5;
            var bid_price_5 = 21;
            var bid_timestamp_5 = DateTimeOffset.Now;
            var bid_order_5 = new LimitOrder(
                bid_orderId_5,
                bid_isBid_5,
                bid_quantity_5,
                bid_price_5,
                bid_timestamp_5);

            orderbook.PlaceOrder(bid_order_1);
            orderbook.PlaceOrder(bid_order_2);
            orderbook.PlaceOrder(bid_order_3);
            orderbook.PlaceOrder(bid_order_4);
            orderbook.PlaceOrder(bid_order_5);
            var orderToRemove = new LimitOrder();
            orderToRemove.SetOrderId(bid_order_3.OrderId);
            orderbook.RemoveOrderByOrderId(orderToRemove);

            //<1>. Should have 3 level of Bids, $23, $22, $21
            //<2>. At level $21, there are two orders left, with Quantity 5 and 5.
            //<3>. TotalLevelQuantity at this level should be 10.

            //Assert <1>
            Assert.IsNotNull(orderbook.BestBidLevel);
            var bestBidLevels = orderbook.GetBestBidLevels(3);
            Assert.AreEqual(3, bestBidLevels.Count);
            Assert.AreEqual(23, bestBidLevels[0].LimitPrice);
            Assert.AreEqual(22, bestBidLevels[1].LimitPrice);
            Assert.AreEqual(21, bestBidLevels[2].LimitPrice);

            //Assert <2>
            var targetLevel = bestBidLevels[2];
            Assert.AreEqual(2, targetLevel.NumberOfOrders);
            var firstBidOrderNode = targetLevel.Orders.List.First;
            Assert.IsNotNull(firstBidOrderNode);
            Assert.AreEqual(5, firstBidOrderNode.Value.Quantity);
            var secondBidOrderNode = firstBidOrderNode.Next;
            Assert.IsNotNull(secondBidOrderNode);
            Assert.AreEqual(5, secondBidOrderNode.Value.Quantity);

            //Assert <3>
            Assert.AreEqual(10, targetLevel.TotalLevelQuantity);
        }

        [TestMethod]
        public void Update_Order_Price()
        {
            orderbook = new FullOrderbook();
            decimal newPrice = 22;

            var bid_orderId_1 = "bid_order_1";
            var bid_isBid_1 = true;
            var bid_quantity_1 = 5;
            var bid_price_1 = 23;
            var bid_timestamp_1 = DateTimeOffset.Now;
            var bid_order_1 = new LimitOrder(
                bid_orderId_1,
                bid_isBid_1,
                bid_quantity_1,
                bid_price_1,
                bid_timestamp_1);

            var bid_orderId_2 = "bid_order_2";
            var bid_isBid_2 = true;
            var bid_quantity_2 = 5;
            var bid_price_2 = 22;
            var bid_timestamp_2 = DateTimeOffset.Now;
            var bid_order_2 = new LimitOrder(
                bid_orderId_2,
                bid_isBid_2,
                bid_quantity_2,
                bid_price_2,
                bid_timestamp_2);

            var bid_orderId_3 = "bid_order_3";
            var bid_isBid_3 = true;
            var bid_quantity_3 = 4;
            var bid_price_3 = 21;
            var bid_timestamp_3 = DateTimeOffset.Now;
            var bid_order_3 = new LimitOrder(
                bid_orderId_3,
                bid_isBid_3,
                bid_quantity_3,
                bid_price_3,
                bid_timestamp_3);

            var bid_orderId_4 = "bid_order_4";
            var bid_isBid_4 = true;
            var bid_quantity_4 = 5;
            var bid_price_4 = 21;
            var bid_timestamp_4 = DateTimeOffset.Now;
            var bid_order_4 = new LimitOrder(
                bid_orderId_4,
                bid_isBid_4,
                bid_quantity_4,
                bid_price_4,
                bid_timestamp_4);

            var bid_orderId_5 = "bid_order_5";
            var bid_isBid_5 = true;
            var bid_quantity_5 = 3;
            var bid_price_5 = 21;
            var bid_timestamp_5 = DateTimeOffset.Now;
            var bid_order_5 = new LimitOrder(
                bid_orderId_5,
                bid_isBid_5,
                bid_quantity_5,
                bid_price_5,
                bid_timestamp_5);

            orderbook.PlaceOrder(bid_order_1);
            orderbook.PlaceOrder(bid_order_2);
            orderbook.PlaceOrder(bid_order_3);
            orderbook.PlaceOrder(bid_order_4);
            orderbook.PlaceOrder(bid_order_5);

            var updated_bid_order_3 = new LimitOrder(
                bid_order_3.OrderId,
                bid_order_3.IsBid,
                bid_order_3.Quantity,
                newPrice,
                DateTimeOffset.Now
                );
            orderbook.PlaceOrder(updated_bid_order_3);

            //<1>. Should have 3 level of Bids, $23, $22, $21
            //<2>. At level $22, there are two orders left, with Quantity 5 and 4.
            //<3>. At level $21, there is two order left, with Quantity 5 and 3.

            //Assert <1>
            Assert.IsNotNull(orderbook.BestBidLevel);
            var bestBidLevels = orderbook.GetBestBidLevels(3);
            Assert.AreEqual(3, bestBidLevels.Count);
            Assert.AreEqual(23, bestBidLevels[0].LimitPrice);
            Assert.AreEqual(22, bestBidLevels[1].LimitPrice);
            Assert.AreEqual(21, bestBidLevels[2].LimitPrice);

            //Assert <2>
            var targetLevel_22 = bestBidLevels[1];
            Assert.AreEqual(2, targetLevel_22.NumberOfOrders);
            var firstBidOrderNode_22 = targetLevel_22.Orders.List.First;
            Assert.IsNotNull(firstBidOrderNode_22);
            Assert.AreEqual(5, firstBidOrderNode_22.Value.Quantity);
            var secondBidOrderNode_22 = firstBidOrderNode_22.Next;
            Assert.IsNotNull(secondBidOrderNode_22);
            Assert.AreEqual(4, secondBidOrderNode_22.Value.Quantity);
            Assert.AreEqual(9, targetLevel_22.TotalLevelQuantity);

            //Assert <3>
            var targetLevel_21 = bestBidLevels[2];
            Assert.AreEqual(2, targetLevel_21.NumberOfOrders);
            var firstBidOrderNode_21 = targetLevel_21.Orders.List.First;
            Assert.IsNotNull(firstBidOrderNode_21);
            Assert.AreEqual(5, firstBidOrderNode_21.Value.Quantity);
            var secondBidOrderNode_21 = firstBidOrderNode_21.Next;
            Assert.IsNotNull(secondBidOrderNode_21);
            Assert.AreEqual(3, secondBidOrderNode_21.Value.Quantity);
            Assert.AreEqual(8, targetLevel_21.TotalLevelQuantity);
        }

        [TestMethod]
        public void Update_Order_Quantity()
        {
            orderbook = new FullOrderbook();
            decimal newQty = 10;

            var bid_orderId_1 = "bid_order_1";
            var bid_isBid_1 = true;
            var bid_quantity_1 = 5;
            var bid_price_1 = 23;
            var bid_timestamp_1 = DateTimeOffset.Now;
            var bid_order_1 = new LimitOrder(
                bid_orderId_1,
                bid_isBid_1,
                bid_quantity_1,
                bid_price_1,
                bid_timestamp_1);

            var bid_orderId_2 = "bid_order_2";
            var bid_isBid_2 = true;
            var bid_quantity_2 = 5;
            var bid_price_2 = 23;
            var bid_timestamp_2 = DateTimeOffset.Now;
            var bid_order_2 = new LimitOrder(
                bid_orderId_2,
                bid_isBid_2,
                bid_quantity_2,
                bid_price_2,
                bid_timestamp_2);

            var bid_orderId_3 = "bid_order_3";
            var bid_isBid_3 = true;
            var bid_quantity_3 = 5;
            var bid_price_3 = 23;
            var bid_timestamp_3 = DateTimeOffset.Now;
            var bid_order_3 = new LimitOrder(
                bid_orderId_3,
                bid_isBid_3,
                bid_quantity_3,
                bid_price_3,
                bid_timestamp_3);

            orderbook.PlaceOrder(bid_order_1);
            orderbook.PlaceOrder(bid_order_2);
            orderbook.PlaceOrder(bid_order_3);

            var updated_bid_order_2 = bid_order_2.DeepCopyWithQtyChanged(newQty);
            orderbook.PlaceOrder(updated_bid_order_2);

            //<1>. Should have only one bid level.
            //<2>. At this level, the Quantities are 5, 10, 5.

            //Assert <1>
            Assert.IsNotNull(orderbook.BestBidLevel);
            var bestBidLevels = orderbook.GetBestBidLevels(3);
            Assert.AreSame(orderbook.BestBidLevel, bestBidLevels[0]);
            Assert.AreEqual(1, bestBidLevels.Count);
            Assert.AreEqual(23, bestBidLevels[0].LimitPrice);

            //Assert <2>
            Assert.AreEqual(3, orderbook.BestBidLevel.NumberOfOrders);
            var firstBidOrderNode = orderbook.BestBidLevel.Orders.List.First;
            Assert.IsNotNull(firstBidOrderNode);
            Assert.AreEqual(5, firstBidOrderNode.Value.Quantity);
            var secondBidOrderNode = firstBidOrderNode.Next;
            Assert.IsNotNull(secondBidOrderNode);
            Assert.AreEqual(newQty, secondBidOrderNode.Value.Quantity);
            var thirdBidOrderNode = secondBidOrderNode.Next;
            Assert.IsNotNull(thirdBidOrderNode);
            Assert.AreEqual(5, thirdBidOrderNode.Value.Quantity);
            Assert.AreEqual(20, orderbook.BestBidLevel.TotalLevelQuantity);
        }
    }
}
