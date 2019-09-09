namespace SyncSoft.StylesDelivered.Event.Inventory
{
    public class ItemInventoryChangedEvent : App.Messaging.Event
    {
        public string SKU { get; private set; }
        public int InvQty { get; private set; }

        public ItemInventoryChangedEvent(string sku, int invQty)
        {
            SKU = sku;
            InvQty = invQty;
        }
    }
}
