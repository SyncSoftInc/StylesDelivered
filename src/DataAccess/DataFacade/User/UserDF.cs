using SyncSoft.App.Components;
using SyncSoft.ECOM.APIs.User;
using SyncSoft.ECOM.DTOs;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.User
{
    public class UserDF : IUserDF
    {
        private static readonly Lazy<IUserDAL> _lazyUserDAL = ObjectContainer.LazyResolve<IUserDAL>();
        private IUserDAL UserDAL => _lazyUserDAL.Value;

        private static readonly Lazy<IUserApi> _lazyUserApi = ObjectContainer.LazyResolve<IUserApi>();
        private IUserApi UserApi => _lazyUserApi.Value;

        public Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId)
            => UserDAL.GetUserAddressesAsync(userId);

        public async Task<UserDTO> GetUserAsync(Guid userId)
        {
            var hr = await UserApi.GetUserAsync(userId).ConfigureAwait(false);
            return await hr.GetResultAsync().ConfigureAwait(false);
        }

        //public Task<PagedList<UserDTO>> GetUsersAsync(GetUsersQuery query)
        //    => UserDAL.GetUsersAsync(query);
    }
}
