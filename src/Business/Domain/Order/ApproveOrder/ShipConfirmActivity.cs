using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Domain.Inventory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.ApproveOrder
{
    public class ShipConfirmActivity : TccActivity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        private static readonly Lazy<IOrderItemDAL> _lazyOrderItemDAL = ObjectContainer.LazyResolve<IOrderItemDAL>();
        private IOrderItemDAL OrderItemDAL => _lazyOrderItemDAL.Value;

        #endregion

        protected override async Task RunAsync(CancellationToken? cancellationToken)
        {
            var cmd = base.Context.Get<ApproveOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);
            var dic = new Dictionary<string, long>();
            var msgCode = MsgCodes.SUCCESS;

            var orderItems = await OrderItemDAL.GetOrderItemsAsync(cmd.OrderNo).ConfigureAwait(false);
            foreach (var item in orderItems)
            {
                var itemInv = ItemInventoryFactory.Create(item.SKU);
                msgCode = await itemInv.ShipConfirmAsync(item.Qty).ConfigureAwait(false);
                if (msgCode.IsSuccess())
                {
                    dic.Add(item.SKU, item.Qty);
                }
                else
                {
                    break;
                }
            }

            // 备份
            Context.Set("ShipConfirmItems", dic);

            if (!msgCode.IsSuccess())
            {
                throw new Exception("Ship confirm failed: " + msgCode);
            }
        }

        protected override async Task RollbackAsync()
        {
            var dic = Context.Get<Dictionary<string, long>>("ShipConfirmItems");
        }
    }
}
