using SyncSoft.StylesDelivered.DTO.Order;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.Event.Order
{
    public class OrderShippedEvent : App.Messaging.Event
    {
        public string OrderNo { get; private set; }
        public IList<OrderItemDTO> OrderItems { get; private set; }

        public OrderShippedEvent(string orderNo, IList<OrderItemDTO> orderItems)
        {
            OrderNo = orderNo;
            OrderItems = orderItems;
        }
    }
}
