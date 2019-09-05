using SyncSoft.StylesDelivered.DataAccess.Inventory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Redis.Inventory
{
    public class InventoryQueryDAL : IInventoryDAL
    {
        private const string _Key = "INV:STYD";
        private readonly IInventoryDB _db;

        public InventoryQueryDAL(IInventoryDB db)
        {
            _db = db;
        }

        public int GetAvailableInventory(string itemNo)
        {
            return _db.HGet<int>(_Key, itemNo);
        }

        //public async Task<int> GetAvailableInventoryAsync(string itemNo)
        //{
        //    return await _db.HGetAsync<int>(_Key, itemNo).ConfigureAwait(false);
        //}

        public async Task<IDictionary<string, int>> GetItemInventoriesAsync(params string[] itemNos)
        {
            if (itemNos.IsMissing())
            {
                return await _db.HGetAllAsync<int>(_Key).ConfigureAwait(false);
            }
            else
            {
                var results = await _db.HMGetAsync<int>(_Key, itemNos).ConfigureAwait(false);

                return Enumerable.Range(0, itemNos.Length).ToDictionary(i => itemNos[i], i => results[i]);
            }
        }

        //public async Task SetItemInventoriesAsync(params (string, int)[] inventories)
        //{
        //    var data = inventories.Select(x => new KeyValuePair<string, object>(x.Item1, x.Item2)).ToArray();

        //    await _db.HMSetAsync(_Key, data).ConfigureAwait(false);
        //}
        public void SetItemInventories(params (string, int)[] inventories)
        {
            var data = inventories.Select(x => new KeyValuePair<string, object>(x.Item1, x.Item2)).ToArray();
            _db.HMSet(_Key, data);
        }
    }
}
