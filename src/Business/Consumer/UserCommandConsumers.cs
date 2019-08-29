using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Command.User;
using SyncSoft.StylesDelivered.Domain.User;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class UserCommandConsumers : 
          IConsumer<RemoveAddressCommand>
        , IConsumer<SaveAddressCommand>
        , IConsumer<SaveUserProfileCommand>
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IUserService> _lazyUserService = ObjectContainer.LazyResolve<IUserService>();
        private IUserService UserService => _lazyUserService.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  User Address  -

        public async Task<object> HandleAsync(IContext<RemoveAddressCommand> context)
        {
            var cmd = context.Message;
            return await UserService.RemoveAddressAsync(cmd).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<SaveAddressCommand> context)
        {
            var cmd = context.Message;
            return await UserService.SaveAddressAsync(cmd).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  User  -

        public async Task<object> HandleAsync(IContext<SaveUserProfileCommand> context)
        {
            return await UserService.SaveProfileAsync(context.Message).ConfigureAwait(false);
        }

        #endregion
    }
}
