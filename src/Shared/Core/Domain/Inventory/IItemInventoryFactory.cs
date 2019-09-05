namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public interface IItemInventoryFactory
    {
        IItemInventory Create(string itemNo);
    }
}
