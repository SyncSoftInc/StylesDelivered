using SyncSoft.StylesDelivered.Command.User;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.User
{
    public interface IUserService
    {
        Task<string> SaveAddressAsync(SaveAddressCommand cmd);
        Task<string> RemoveAddressAsync(RemoveAddressCommand cmd);

        Task<string> CreateProfileAsync(CreateUserProfileCommand message);
        Task<string> SaveProfileAsync(SaveUserProfileCommand message);
        Task<string> DeleteProfileAsync(DeleteUserProfileCommand message);
    }
}
