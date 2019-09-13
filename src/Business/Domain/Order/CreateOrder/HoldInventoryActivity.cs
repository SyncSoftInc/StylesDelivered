using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.Domain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class HoldInventoryActivity : TccActivity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        #endregion

        protected override async Task RunAsync(CancellationToken? cancellationToken)
        {
            var cmd = base.Context.Get<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);
            var dic = new Dictionary<string, long>();
            string msgCode = MsgCodes.SUCCESS;

            foreach (var orderItem in cmd.Order.Items)
            {
                var itemInv = ItemInventoryFactory.Create(orderItem.SKU);
                msgCode = await itemInv.HoldAsync(orderItem.Qty).ConfigureAwait(false);
                if (msgCode.IsSuccess())
                {
                    dic.Add(orderItem.SKU, orderItem.Qty);
                }
                else
                {
                    break;
                }
            }

            // 备份
            Context.Set("HoldItems", dic);

            if (!msgCode.IsSuccess())
            {
                throw new Exception("Hold inventory failed: " + msgCode);
            }
        }

        protected override async Task RollbackAsync()
        {
            var dic = Context.Get<Dictionary<string, long>>("HoldItems");
            if (dic.IsPresent())
            {
                foreach (var kvp in dic)
                {
                    var itemInv = ItemInventoryFactory.Create(kvp.Key);
                    var msgCode = await itemInv.UnholdAsync(kvp.Value).ConfigureAwait(false);
                    if (!msgCode.IsSuccess())
                    {
                        throw new Exception($"Unhold inventory {kvp.Key}:{kvp.Value} failed");
                    }
                }
            }
        }
    }
}
