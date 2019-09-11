using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public interface IItemInventory
    {
        Task<long> GetOnHandAsync();
        Task<string> SetOnHandAsync(long qty);
        Task<string> HoldAsync(long qty);
        Task<string> UnholdAsync(long qty);
    }
}
