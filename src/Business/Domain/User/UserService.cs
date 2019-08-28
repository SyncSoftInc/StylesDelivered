using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Command.User;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DTO.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.User
{
    public class UserService : IUserService
    {
        private static readonly Lazy<IUserDAL> _lazyUserDAL = ObjectContainer.LazyResolve<IUserDAL>();
        private IUserDAL UserDAL => _lazyUserDAL.Value;

        public async Task<string> RemoveAddressAsync(RemoveAddressCommand cmd)
        {
            var userId = cmd.Identity.UserID();
            var user = await GetOrInitUserAsync(userId).ConfigureAwait(false);
            var address = user.Addresses.FirstOrDefault(x => x.ID == cmd.AddressID);
            if (address.IsNotNull())
            {
                user.Addresses.Remove(address);
            }
            return await UserDAL.UpdateUserAddressesAsync(userId, user.Addresses).ConfigureAwait(false);
        }

        public async Task<string> SaveAddressAsync(SaveAddressCommand cmd)
        {
            // 格式化地址
            cmd.Address.Address1 = Utils.FormatAddress(cmd.Address.Address1);
            cmd.Address.Address2 = Utils.FormatAddress(cmd.Address.Address2);
            cmd.Address.City = Utils.FormatAddress(cmd.Address.City);
            cmd.Address.State = Utils.FormatAddress(cmd.Address.State);
            cmd.Address.ZipCode = Utils.FormatAddress(cmd.Address.ZipCode);

            var userId = cmd.Identity.UserID();
            var user = await GetOrInitUserAsync(userId).ConfigureAwait(false);
            if (!user.Addresses.Contains(cmd.Address))
            {// 没有才添加
                cmd.Address.ID = cmd.Address.ToSha1();
                user.Addresses.Add(cmd.Address);
            }
            else
            {
                return MsgCodes.AddressExists;
            }

            return await UserDAL.UpdateUserAddressesAsync(userId, user.Addresses).ConfigureAwait(false);
        }

        public async Task<UserDTO> GetOrInitUserAsync(Guid userId)
        {
            var user = await UserDAL.GetUserAsync(userId).ConfigureAwait(false);
            if (user.IsNull())
            {
                user = new UserDTO { ID = userId };
                await UserDAL.InsertUserAsync(user).ConfigureAwait(false);
            }
            return user;
        }
    }
}
