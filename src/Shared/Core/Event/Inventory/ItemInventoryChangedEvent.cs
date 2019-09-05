namespace SyncSoft.StylesDelivered.Event.Inventory
{
    public class ItemInventoryChangedEvent : App.Messaging.Event
    {
        public string ItemNo { get; private set; }
        public int InvQty { get; private set; }

        public ItemInventoryChangedEvent(string itemNo, int invQty)
        {
            ItemNo = itemNo;
            InvQty = invQty;
        }
    }
}
