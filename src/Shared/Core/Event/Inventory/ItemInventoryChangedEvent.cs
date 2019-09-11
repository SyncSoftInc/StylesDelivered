namespace SyncSoft.StylesDelivered.Event.Inventory
{
    public class ItemInventoryChangedEvent : App.Messaging.Event
    {
        public string SKU { get; private set; }
        public long InvQty { get; private set; }

        public ItemInventoryChangedEvent(string sku, long invQty)
        {
            SKU = sku;
            InvQty = invQty;
        }
    }
}
