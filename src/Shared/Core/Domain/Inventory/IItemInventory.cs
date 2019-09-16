using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public interface IItemInventory
    {
        Task<long> GetOnHandAsync();
        Task<string> SetOnHandAsync(long qty);
        Task<string> HoldAsync(Guid correlationid, long qty);
        Task<string> UnholdAsync(long qty);
        Task<long> GetAvbQtyAsync();
    }
}
