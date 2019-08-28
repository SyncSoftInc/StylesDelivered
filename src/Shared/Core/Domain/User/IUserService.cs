using SyncSoft.StylesDelivered.Command.User;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.User
{
    public interface IUserService
    {
        Task<string> SaveAddressAsync(SaveAddressCommand cmd);
        Task<string> RemoveAddressAsync(RemoveAddressCommand cmd);
    }
}
