namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public interface IItemInventory
    {
        /// <summary>
        /// 检查库存是否足够
        /// </summary>
        bool IsAvailable(int qty);

        /// <summary>
        /// 获取库存数量
        /// </summary>
        int Get();

        /// <summary>
        /// 设置库存数量
        /// </summary>
        string Set(int invQty);
    }
}
