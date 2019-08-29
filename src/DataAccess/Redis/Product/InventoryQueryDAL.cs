using SyncSoft.StylesDelivered.DataAccess.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Redis.Product.Inventory
{
    public class InventoryQueryDAL : IInventoryQueryDAL
    {
        private const string _Key = "INV:STYD";
        private readonly IInventoryDB _db;

        public InventoryQueryDAL(IInventoryDB db)
        {
            _db = db;
        }

        public async Task<int> GetAvailableInventoryAsync(string itemNo)
        {
            return await _db.HGetAsync<int>(_Key, itemNo).ConfigureAwait(false);
        }

        public async Task<IDictionary<string, int>> GetAvailableInventoriesAsync(params string[] itemNos)
        {
            var results = await _db.HMGetAsync<int>(_Key, itemNos).ConfigureAwait(false);

            return Enumerable.Range(0, itemNos.Length).ToDictionary(i => itemNos[i], i => results[i]);
        }

        public async Task SyncInventoriesAsync(KeyValuePair<string, int>[] inventories)
        {
            var data = inventories.Select(x => new KeyValuePair<string, object>(x.Key, x.Value)).ToArray();
            await _db.HMSetAsync(_Key, data).ConfigureAwait(false);
        }
    }
}
