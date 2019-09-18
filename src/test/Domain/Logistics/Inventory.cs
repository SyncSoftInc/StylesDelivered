using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered;
using System;
using System.Threading.Tasks;

namespace Logistics
{
    public class Inventory
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -


        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        #endregion

        [Test]
        public async Task CleanWarehouse()
        {
            var mr = await InventoryServiceClient.CleanWarehouseAsync(new InventoriesMSG { Warehouse = Constants.WarehouseID });
        }
    }
}
