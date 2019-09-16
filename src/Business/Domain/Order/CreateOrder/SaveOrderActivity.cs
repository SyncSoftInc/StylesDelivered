using Logistics;
using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class SaveOrderActivity : TccActivity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        #endregion

        protected override async Task RunAsync(CancellationToken? cancellationToken)
        {
            var cmd = base.Context.Get<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);
            var userId = cmd.Identity.UserID();

            // Check Pending Order
            var order = await OrderDAL.GetPendingOrderAsync(userId).ConfigureAwait(false);
            if (order.IsNotNull())
            {// User has pending order
                var err = $"Pending order exists.";
                Context.Set(CreateOrderTransaction.Error, err);
                throw new Exception(err);
            }

            // 检查库存
            var invQ = new InventoriesDTO { Warehouse = Constants.WarehouseID };
            invQ.Inventories.AddRange(cmd.Order.Items.Select(x => new InventoryDTO { ItemNo = x.SKU }));
            invQ = await InventoryServiceClient.GetAvbQtysAsync(invQ);
            var invs = invQ.Inventories.Where(x => x.Qty <= 0);
            if (invs.IsPresent())
            {// 有Item库存不足
                var err = $"Item(s):[{invs.Select(x => x.ItemNo).JointStrings()}] do(es)n't have enough inventories.";
                Context.Set(CreateOrderTransaction.Error, err);
                throw new Exception(err);
            }

            cmd.Order.OrderNo = Guid.NewGuid().ToLowerNString();
            Context.Set("OrderNo", cmd.Order.OrderNo);
            cmd.Order.User_ID = userId;
            cmd.Order.Status = OrderStatusEnum.Pending;

            foreach (var orderItem in cmd.Order.Items)
            {
                var item = await ProductItemDAL.GetItemAsync(orderItem.ASIN, orderItem.SKU).ConfigureAwait(false);
                if (item.IsNull())
                {
                    var err = $"Item '{orderItem.SKU}' doesn't exists.";
                    Context.Set(CreateOrderTransaction.Error, err);
                    throw new Exception(err);
                }
                orderItem.OrderNo = cmd.Order.OrderNo;
                orderItem.Alias = item.Alias;
                orderItem.Color = item.Color;
                orderItem.Size = item.Size;
                orderItem.Url = item.Url;
                orderItem.Qty = 1;
            }

            await OrderDAL.InsertAsync(cmd.Order).ConfigureAwait(false);
        }

        protected override async Task RollbackAsync()
        {
            var orderNo = await Context.GetAsync<string>("OrderNo").ConfigureAwait(false);
            await OrderDAL.DeleteOrderAsync(orderNo).ConfigureAwait(false);
        }
    }
}
