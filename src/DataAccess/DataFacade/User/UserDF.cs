using SyncSoft.App.Components;
using SyncSoft.ECP.DTOs.Users;
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

        public Task<IList<AddressDTO>> GetUserAddressesAsync(Guid userId)
            => UserDAL.GetUserAddressesAsync(userId);

        public async Task<UserBasicInfoDTO> GetUserBasicInfoAsync(Guid userId)
        {
            var user = await UserDAL.GetUserAsync(userId).ConfigureAwait(false);
            if (user.IsNotNull())
            {
                return new UserBasicInfoDTO
                {
                    ID = user.ID,
                    Email = user.Email,
                    Status = user.Status,
                    Roles = (long?)user.Roles,
                };
            }
            else
            {
                return null;
            }
        }
    }
}
