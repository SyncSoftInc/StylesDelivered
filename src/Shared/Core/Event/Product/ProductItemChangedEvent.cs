namespace SyncSoft.StylesDelivered.Event.Product
{
    public class ProductItemChangedEvent : App.Messaging.Event
    {
        public string ASIN { get; private set; }

        public ProductItemChangedEvent(string asin)
        {
            ASIN = asin;
        }
    }
}
