using SyncSoft.StylesDelivered.Command.User;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.User
{
    public interface IUserService
    {
        // *******************************************************************************************************************************
        #region -  User  -

        Task<string> SaveProfileAsync(SaveUserProfileCommand message);

        #endregion
        // *******************************************************************************************************************************
        #region -  AdminUser  -

        Task<string> CreateAdminUserAsync(CreateAdminUserCommand message);
        Task<string> UpdateAdminUserAsync(SaveAdminUserCommand message);
        Task<string> DeleteAdminUserAsync(DeleteAdminUserCommand message);

        #endregion
        // *******************************************************************************************************************************
        #region -  Address  -

        Task<string> SaveAddressAsync(SaveAddressCommand cmd);
        Task<string> RemoveAddressAsync(RemoveAddressCommand cmd);

        #endregion
    }
}
