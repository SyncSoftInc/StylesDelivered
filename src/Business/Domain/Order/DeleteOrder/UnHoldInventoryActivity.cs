//using SyncSoft.App;
//using SyncSoft.App.Components;
//using SyncSoft.App.Transactions;
//using SyncSoft.StylesDelivered.Command.Order;
//using SyncSoft.StylesDelivered.DataAccess.Order;
//using SyncSoft.StylesDelivered.Domain.Inventory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SyncSoft.StylesDelivered.Domain.Order.DeleteOrder
//{
//    public class UnHoldInventoryActivity : Activity
//    {
//        // *******************************************************************************************************************************
//        #region -  Lazy Object(s)  -

//        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
//        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

//        private static readonly Lazy<IOrderItemDAL> _lazyOrderItemDAL = ObjectContainer.LazyResolve<IOrderItemDAL>();
//        private IOrderItemDAL OrderItemDAL => _lazyOrderItemDAL.Value;

//        #endregion

//        protected override async Task<string> RunAsync()
//        {
//            var cmd = await GetStateAsync<DeleteOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
//            var dic = new Dictionary<string, long>();
//            var msgCode = MsgCodes.SUCCESS;

//            var orderItems = await OrderItemDAL.GetOrderItemsAsync(cmd.OrderNo).ConfigureAwait(false);
//            foreach (var item in orderItems)
//            {
//                var itemInv = ItemInventoryFactory.Create(item.SKU);
//                msgCode = await itemInv.UnholdAsync(item.Qty).ConfigureAwait(false);
//                if (msgCode.IsSuccess())
//                {
//                    dic.Add(item.SKU, item.Qty);
//                }
//                else
//                {
//                    break;
//                }
//            }

//            // 备份
//            await SetStateAsync("UnHoldItems", dic).ConfigureAwait(false);

//            return msgCode;
//        }

//        protected override async Task<string> RollbackAsync()
//        {
//            var msgCode = MsgCodes.SUCCESS;
//            var dic = await GetStateAsync<Dictionary<string, long>>("UnHoldItems").ConfigureAwait(false);
//            if (dic.IsPresent())
//            {
//                foreach (var item in dic)
//                {
//                    var itemInv = ItemInventoryFactory.Create(item.Key);
//                    msgCode = await itemInv.HoldAsync(Transaction.ID, item.Value).ConfigureAwait(false);
//                    if (msgCode.IsSuccess()) break;
//                    // ^^^^^^^^^^
//                }
//            }

//            return msgCode;
//        }
//    }
//}
