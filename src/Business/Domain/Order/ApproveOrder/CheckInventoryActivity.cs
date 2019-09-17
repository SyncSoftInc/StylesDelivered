using Logistics;
using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.ApproveOrder
{
    public class CheckInventoryActivity : Activity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        #endregion

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<ApproveOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
            // Get Order
            var order = await OrderDAL.GetOrderAsync(cmd.OrderNo).ConfigureAwait(false);
            if (order.IsNull())
            {
                return MsgCodes.OrderNotExists;
            }

            // 检查库存
            var invQ = new InventoriesDTO { Warehouse = Constants.WarehouseID };
            invQ.Inventories.AddRange(order.Items.Select(x => new InventoryDTO { ItemNo = x.SKU }));
            invQ = await InventoryServiceClient.GetAvbQtysAsync(invQ);
            var invs = invQ.Inventories.Where(x => x.Qty <= 0);
            if (invs.IsPresent())
            {// 有Item库存不足
                var msgCode = $"Item(s):[{invs.Select(x => x.ItemNo).JointStrings()}] do(es)n't have enough inventories.";
                return msgCode;
            }

            return MsgCodes.SUCCESS;
        }
    }
}
