using Logistics;
using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.ApproveOrder
{
    public class ApproveOrderActivity : TccActivity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        #endregion

        protected override async Task RunAsync(CancellationToken? cancellationToken)
        {
            var cmd = base.Context.Get<ApproveOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);

            // Get Order
            var order = await OrderDAL.GetOrderAsync(cmd.OrderNo).ConfigureAwait(false);
            if (order.IsNull())
            {
                var err = MsgCodes.OrderNotExists;
                Context.Set(ApproveOrderTransaction.Error, err);
                throw new Exception(err);
            }
            else if (order.IsNotNull() && order.Status == OrderStatusEnum.Approved)
            {// order is approved
                var err = $"Order already approved.";
                Context.Set(ApproveOrderTransaction.Error, err);
                throw new Exception(err);
            }

            await OrderDAL.UpdateOrderStatusAsync(cmd.OrderNo, OrderStatusEnum.Approved).ConfigureAwait(false);
        }

        protected override async Task RollbackAsync()
        {
            var cmd = base.Context.Get<ApproveOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);
            await OrderDAL.UpdateOrderStatusAsync(cmd.OrderNo, OrderStatusEnum.Pending).ConfigureAwait(false);
        }
    }
}
