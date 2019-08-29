using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Product
{
    public interface IInventoryQueryDAL
    {
        Task<int> GetAvailableInventoryAsync(string itemNo);
        Task<IDictionary<string, int>> GetAvailableInventoriesAsync(params string[] itemNos);
        Task SyncInventoriesAsync(KeyValuePair<string, int>[] inventories);
    }
}
