using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.Domain.User;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class SaveUserAddressActivity : Activity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IUserService> _lazyUserService = ObjectContainer.LazyResolve<IUserService>();
        private IUserService UserService => _lazyUserService.Value;

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<SaveUserAddressActivity>();
        private ILogger Logger => _lazyLogger.Value;

        #endregion

        protected override async Task<string> RunAsync()
        {
            var cmd = await GetStateAsync<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand).ConfigureAwait(false);

            var saveAddressCmd = new Command.User.SaveAddressCommand
            {
                Address = new DTO.Common.AddressDTO
                {
                    User_ID = cmd.Identity.UserID(),
                    Address1 = cmd.Order.Shipping_Address1,
                    Address2 = cmd.Order.Shipping_Address2,
                    City = cmd.Order.Shipping_City,
                    State = cmd.Order.Shipping_State,
                    ZipCode = cmd.Order.Shipping_ZipCode,
                    Country = cmd.Order.Shipping_Country,
                }
            };
            saveAddressCmd.SetContext(cmd);

            var msgCode = await UserService.SaveAddressAsync(saveAddressCmd).ConfigureAwait(false);
            if (!msgCode.IsSuccess() && msgCode != MsgCodes.AddressExists)
            {
                Logger.Warn("Save address failed. \n{0}", saveAddressCmd.Address);
            }
            return MsgCodes.SUCCESS;    // 不需要影响主逻辑，永远认为是成功
        }
    }
}
