//using SyncSoft.App;
//using SyncSoft.App.Components;
//using SyncSoft.App.Transactions;
//using SyncSoft.StylesDelivered.Command.Order;
//using SyncSoft.StylesDelivered.Domain.Inventory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
//{
//    public class HoldInventoryActivity : Activity
//    {
//        // *******************************************************************************************************************************
//        #region -  Lazy Object(s)  -

//        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
//        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

//        #endregion

//        protected override async Task<string> RunAsync()
//        {
//            var cmd = await GetStateAsync<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
//            var dic = new Dictionary<string, long>();
//            string msgCode = MsgCodes.SUCCESS;

//            foreach (var orderItem in cmd.Order.Items)
//            {
//                var itemInv = ItemInventoryFactory.Create(orderItem.SKU);
//                msgCode = await itemInv.HoldAsync(Transaction.ID, orderItem.Qty).ConfigureAwait(false);
//                if (msgCode.IsSuccess())
//                {
//                    dic.Add(orderItem.SKU, orderItem.Qty);
//                }
//                else
//                {
//                    break;
//                }
//            }

//            // 备份
//            await SetStateAsync("HoldItems", dic).ConfigureAwait(false);

//            return msgCode;
//        }

//        protected override async Task<string> RollbackAsync()
//        {
//            var msgCode = MsgCodes.SUCCESS;
//            var dic = await GetStateAsync<Dictionary<string, long>>("HoldItems").ConfigureAwait(false);
//            if (dic.IsPresent())
//            {
//                foreach (var kvp in dic)
//                {
//                    var itemInv = ItemInventoryFactory.Create(kvp.Key);
//                    msgCode = await itemInv.UnholdAsync(kvp.Value).ConfigureAwait(false);
//                    if (!msgCode.IsSuccess()) break;
//                    // ^^^^^^^^^^
//                }
//            }

//            return msgCode;
//        }
//    }
//}
