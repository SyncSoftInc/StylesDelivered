using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.ShipOrder
{
    public class ChangeOrderStatusActivity : Activity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        #endregion

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<ShipOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
            return await OrderDAL.UpdateOrderStatusAsync(cmd.OrderNo, OrderStatusEnum.Shipped).ConfigureAwait(false);
        }

        protected override async Task<string> RollbackAsync()
        {
            var cmd = await GetStateAsync<ShipOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
            return await OrderDAL.UpdateOrderStatusAsync(cmd.OrderNo, OrderStatusEnum.Approved).ConfigureAwait(false);
        }
    }
}
