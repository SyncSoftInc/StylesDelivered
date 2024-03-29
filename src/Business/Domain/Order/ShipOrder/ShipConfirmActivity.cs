﻿using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Domain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.ShipOrder
{
    public class ShipConfirmActivity : Activity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        private static readonly Lazy<IOrderItemDAL> _lazyOrderItemDAL = ObjectContainer.LazyResolve<IOrderItemDAL>();
        private IOrderItemDAL OrderItemDAL => _lazyOrderItemDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  RunAsync  -

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<ShipOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
            var orderItems = await OrderItemDAL.GetOrderItemsAsync(cmd.OrderNo).ConfigureAwait(false);
            if (orderItems.IsMissing()) return MsgCodes.OrderItemsMissing;
            // ^^^^^^^^^^
            SetResult(orderItems);

            var msgCode = MsgCodes.SUCCESS;

            var dic = new Dictionary<string, long>();
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
            await SetStateAsync("ShipConfirmItems", dic).ConfigureAwait(false);

            return msgCode;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  RollbackAsync  -

        protected override async Task<string> RollbackAsync()
        {
            var msgCode = MsgCodes.SUCCESS;
            var dic = await GetStateAsync<Dictionary<string, long>>("ShipConfirmItems").ConfigureAwait(false);
            if (dic.IsPresent())
            {
                foreach (var kvp in dic)
                {
                    var itemInv = ItemInventoryFactory.Create(kvp.Key);
                    msgCode = await itemInv.CancelShipConfirmAsync(kvp.Value).ConfigureAwait(false);
                    if (!msgCode.IsSuccess()) break;
                    // ^^^^^^^^^^
                }
            }
            return msgCode;
        }

        #endregion
    }
}
