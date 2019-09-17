using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Settings;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.Domain.Inventory;
using SyncSoft.StylesDelivered.DTO.Common;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class CheckEligibilityActivity : Activity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        private static readonly Lazy<ISettingProvider> _lazySettingProvider = ObjectContainer.LazyResolve<ISettingProvider>();
        private ISettingProvider SettingProvider => _lazySettingProvider.Value;

        #endregion

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);
            var userId = cmd.Identity.UserID();

            var setting = await SettingProvider.GetSettingAsync<SettingDTO>().ConfigureAwait(false);

            // Check Pending Order
            var count = await OrderDAL.CountPendingOrderAsync(userId).ConfigureAwait(false);
            if (count >= setting.MaxPendingOrder)
            {// User has pending order
                var err = $"One user can only claim {setting.MaxPendingOrder} items before they get approved.";
                return err;
            }

            return MsgCodes.SUCCESS;
        }
    }
}
