using Logistics;
using SyncSoft.App.Components;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public class ItemInventory : IItemInventory
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

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
            var r = await InventoryServiceClient.GetOnHandQtyAsync(new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku });
            return r.Qty;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SetOnHold  -

        public async Task<string> SetOnHandAsync(long qty)
        {
            var r = await InventoryServiceClient.SetOnHandQtyAsync(new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku, Qty = qty });
            return r.MsgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Hold  -

        public async Task<string> HoldAsync(Guid correlationId, long qty)
        {
            var r = await InventoryServiceClient.HoldAsync(new InventoryDTO
            {
                RequestID = correlationId.ToLowerNString() + "-" + _sku,
                Warehouse = Constants.WarehouseID,
                ItemNo = _sku,
                Qty = qty
            });
            return r.MsgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Unhold  -

        public async Task<string> UnholdAsync(long qty)
        {
            var r = await InventoryServiceClient.UnholdAsync(new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku, Qty = qty });
            return r.MsgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  GetAvbQty  -

        public async Task<long> GetAvbQtyAsync()
        {
            var query = new InventoryDTO { Warehouse = Constants.WarehouseID, ItemNo = _sku };
            query = await InventoryServiceClient.GetAvbQtyAsync(query);
            return query.Qty;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  ShipConfirm  -

        public async Task<string> ShipConfirmAsync(long qty)
        {
            var invs = new InventoriesDTO { Warehouse = Constants.WarehouseID };
            invs.Inventories.Add(new InventoryDTO { ItemNo = _sku, Qty = qty });
            var r = await InventoryServiceClient.ShipConfirmAsync(invs);
            return r.MsgCode;
        }

        #endregion
    }
}
