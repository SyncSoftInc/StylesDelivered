using Logistics;
using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.DeleteOrder
{
    public class DeleteOrderActivity : TccActivity
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

        protected override async Task RunAsync(CancellationToken? cancellationToken)
        {
            var cmd = base.Context.Get<DeleteOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);

            // Get Order
            var order = await OrderDAL.GetOrderAsync(cmd.OrderNo).ConfigureAwait(false);
            order.Items = await OrderItemDAL.GetOrderItemsAsync(cmd.OrderNo).ConfigureAwait(false);

            if (order.IsNull())
            {
                var err = MsgCodes.OrderNotExists;
                Context.Set(DeleteOrderTransaction.Error, err);
                throw new Exception(err);
            }
            else if (order.IsNotNull() && order.Status == OrderStatusEnum.Approved)
            {// order is approved
                var err = $"Cannot delete approved order.";
                Context.Set(DeleteOrderTransaction.Error, err);
                throw new Exception(err);
            }

            // 备份
            Context.Set("Order", order);

            await OrderDAL.DeleteOrderAsync(cmd.OrderNo).ConfigureAwait(false);
        }

        protected override async Task RollbackAsync()
        {
            var order = Context.Get<OrderDTO>("Order");
            if (order.IsNotNull()) await OrderDAL.InsertAsync(order).ConfigureAwait(false);
        }
    }
}
