using Inventory;
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
        // *******************************************************************************************************************************
        #region -  Run  -

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
            var userId = cmd.Identity.UserID();

            cmd.Order.OrderNo = Guid.NewGuid().ToLowerNString();
            SetResult(cmd.Order.OrderNo);   // 设置事务返回结果
            await SetStateAsync("OrderNo", cmd.Order.OrderNo).ConfigureAwait(false);

            cmd.Order.User_ID = userId;
            cmd.Order.User = cmd.Identity.UserNickName();
            cmd.Order.Status = OrderStatusEnum.Pending;
            cmd.Order.CreatedOnUtc = DateTime.UtcNow;
            cmd.Order.Shipping_Address1 = Utils.FormatAddress(cmd.Order.Shipping_Address1);
            cmd.Order.Shipping_Address2 = Utils.FormatAddress(cmd.Order.Shipping_Address2);
            cmd.Order.Shipping_City = Utils.FormatAddress(cmd.Order.Shipping_City);
            cmd.Order.Shipping_State = Utils.FormatAddress(cmd.Order.Shipping_State);
            cmd.Order.Shipping_ZipCode = Utils.FormatAddress(cmd.Order.Shipping_ZipCode);

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

        #endregion
        // *******************************************************************************************************************************
        #region -  Rollback  -

        protected override async Task<string> RollbackAsync()
        {
            var orderNo = await GetStateAsync<string>("OrderNo").ConfigureAwait(false);
            return await OrderDAL.DeleteOrderAsync(orderNo).ConfigureAwait(false);
        }

        #endregion
    }
}
