namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class ItemInventoryFactory : IItemInventoryFactory
    {
        public IItemInventory Create(string itemNo) => new ItemInventory(itemNo);
    }
}
