using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMatchingExchange
{
    public class LimitOrdersLinkedList
    {
        public LinkedList<LimitOrder> List { get; private set; }
        public Dictionary<string, LinkedListNode<LimitOrder>> Table { get; private set; }

        public LimitOrdersLinkedList()
        {
            List = new LinkedList<LimitOrder>();
            Table = new Dictionary<string, LinkedListNode<LimitOrder>>();
        }

        public void AddFirst(LimitOrder order)
        {
            var node = new LinkedListNode<LimitOrder>(order);
            List.AddFirst(node);
            Table.Add(order.OrderId, node);
        }

        public void AddLast(LimitOrder order)
        {
            var node = new LinkedListNode<LimitOrder>(order);
            List.AddLast(node);
            Table.Add(order.OrderId, node);
        }

        public bool Contains(string orderId, out LimitOrder order)
        {
            LinkedListNode<LimitOrder> node;
            bool contain = Table.TryGetValue(orderId, out node);
            if (contain)
            {
                order = node.Value;
            }
            else
            {
                order = null;
            }
            return contain;
        }

        public bool Remove(string orderId, out LimitOrder order)
        {
            LinkedListNode<LimitOrder> node;
            bool contain = Table.TryGetValue(orderId, out node);
            if (contain)
            {
                order = node.Value;
                List.Remove(node);
                if (!Table.Remove(order.OrderId))
                {
                    throw new Exception(string.Format("Can't find orderId = {0} when removing!", order.OrderId));
                }
            }
            else
            {
                order = null;
            }
            return contain;
        }

        public bool Update(LimitOrder order, out decimal prevQty)
        {
            prevQty = 0;
            LinkedListNode<LimitOrder> node;
            bool contain = Table.TryGetValue(order.OrderId, out node);
            if (contain)
            {
                prevQty = node.Value.Quantity;
                node.Value = order;
            }
            return contain;
        }

        public bool Any()
        {
            var tableAny = Table.Any();
            var listAny = List.Any();
            if (tableAny != listAny)
            {
                throw new Exception(string.Format("Any: tableAny = {0} and listAny = {1} doesnt match!", tableAny, listAny));
            }
            else
            {
                return tableAny;
            }
        }
    }
}
