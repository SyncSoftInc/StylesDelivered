namespace SyncSoft.StylesDelivered.Event.Order
{
    public class OrderShippedEvent : App.Messaging.Event
    {
        public string OrderNo { get; private set; }

        public OrderShippedEvent(string orderNo)
        {
            OrderNo = orderNo;
        }
    }
}
