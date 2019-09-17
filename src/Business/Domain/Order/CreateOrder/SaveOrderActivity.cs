using Logistics;
using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class SaveOrderActivity : Activity
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

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
            var userId = cmd.Identity.UserID();

            //// Check Pending Order
            //var order = await OrderDAL.GetPendingOrderAsync(userId).ConfigureAwait(false);
            //if (order.IsNotNull())
            //{// User has pending order
            //    var err = $"One user can only claim one item before it get approved.";
            //    return err;
            //}

            //// 检查库存
            //var invQ = new InventoriesDTO { Warehouse = Constants.WarehouseID };
            //invQ.Inventories.AddRange(cmd.Order.Items.Select(x => new InventoryDTO { ItemNo = x.SKU }));
            //invQ = await InventoryServiceClient.GetAvbQtysAsync(invQ);
            //var invs = invQ.Inventories.Where(x => x.Qty <= 0);
            //if (invs.IsPresent())
            //{// 有Item库存不足
            //    var err = $"Item(s):[{invs.Select(x => x.ItemNo).JointStrings()}] do(es)n't have enough inventories.";
            //    return err;
            //}

            cmd.Order.OrderNo = Guid.NewGuid().ToLowerNString();
            await SetStateAsync("OrderNo", cmd.Order.OrderNo).ConfigureAwait(false);

            cmd.Order.User_ID = userId;
            cmd.Order.User = cmd.Identity.UserFirstName() + " " + cmd.Identity.UserLastName();
            cmd.Order.Status = OrderStatusEnum.Pending;
            cmd.Order.CreatedOnUtc = DateTime.UtcNow;

            foreach (var orderItem in cmd.Order.Items)
            {
                var item = await ProductItemDAL.GetItemAsync(orderItem.ASIN, orderItem.SKU).ConfigureAwait(false);
                if (item.IsNull())
                {
                    var err = $"Item '{orderItem.SKU}' doesn't exists.";
                    return err;
                }
                orderItem.OrderNo = cmd.Order.OrderNo;
                orderItem.Alias = item.Alias;
                orderItem.Color = item.Color;
                orderItem.Size = item.Size;
                orderItem.ImageUrl = item.ImageUrl;
                orderItem.Url = item.Url;
                orderItem.Qty = orderItem.Qty;
            }

            return await OrderDAL.InsertAsync(cmd.Order).ConfigureAwait(false);
        }

        protected override async Task<string> RollbackAsync()
        {
            var orderNo = await GetStateAsync<string>("OrderNo").ConfigureAwait(false);
            return await OrderDAL.DeleteOrderAsync(orderNo).ConfigureAwait(false);
        }
    }
}
