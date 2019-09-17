using Logistics;
using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.DeleteOrder
{
    public class DeleteOrderActivity : Activity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        private static readonly Lazy<IOrderItemDAL> _lazyOrderItemDAL = ObjectContainer.LazyResolve<IOrderItemDAL>();
        private IOrderItemDAL OrderItemDAL => _lazyOrderItemDAL.Value;

        #endregion

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<DeleteOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);

            // Get Order
            var order = await OrderDAL.GetOrderAsync(cmd.OrderNo).ConfigureAwait(false);
            order.Items = await OrderItemDAL.GetOrderItemsAsync(cmd.OrderNo).ConfigureAwait(false);

            if (order.IsNull())
            {
                return MsgCodes.OrderNotExists;
            }
            else if (order.IsNotNull() && order.Status == OrderStatusEnum.Approved)
            {// order is approved
                var err = $"Cannot delete approved order.";
                return err;
            }

            // 备份
            await SetStateAsync("Order", order).ConfigureAwait(false);

            return await OrderDAL.DeleteOrderAsync(cmd.OrderNo).ConfigureAwait(false);
        }

        protected override async Task<string> RollbackAsync()
        {
            var order = await GetStateAsync<OrderDTO>("Order").ConfigureAwait(false);
            if (order.IsNotNull())
            {
                return await OrderDAL.InsertAsync(order).ConfigureAwait(false);
            }

            return MsgCodes.SUCCESS;
        }
    }
}
