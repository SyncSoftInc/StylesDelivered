using SyncSoft.App.Components;
using System;
using System.Threading.Tasks;
using Warehouse;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class ItemInventory : IItemInventory
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<Warehouse.Inventory.InventoryClient> _lazyInventoryService = ObjectContainer.LazyResolve<Warehouse.Inventory.InventoryClient>();
        private Warehouse.Inventory.InventoryClient InventoryService => _lazyInventoryService.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        private readonly string _sku;

        #endregion
        // *******************************************************************************************************************************
        #region -  Constructor(s)  -

        internal ItemInventory(string sku)
        {
            _sku = sku.Trim().ToUpper();
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  GetOnHand  -

        public async Task<long> GetOnHandAsync()
        {
            var r = await InventoryService.GetOnHandQtyAsync(new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku });
            return r.Qty;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SetOnHold  -

        public async Task<string> SetOnHandAsync(long qty)
        {
            var r = await InventoryService.SetOnHandQtyAsync(new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku, Qty = qty });
            return r.MsgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Hold  -

        public async Task<string> HoldAsync(long qty)
        {
            var r = await InventoryService.HoldAsync(new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku, Qty = qty });
            return r.MsgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Hold  -

        public async Task<string> UnholdAsync(long qty)
        {
            var r = await InventoryService.UnholdAsync(new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku, Qty = qty });
            return r.MsgCode;
        }

        #endregion
    }
}
