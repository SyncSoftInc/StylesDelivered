using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public interface IItemInventory
    {
        /// <summary>
        /// 检查库存是否足够
        /// </summary>
        Task<bool> IsAvailableAsync(int qty);
    }
}
