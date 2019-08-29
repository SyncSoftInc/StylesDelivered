using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Command.User;
using SyncSoft.StylesDelivered.DataAccess.User;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.User
{
    public class UserService : IUserService
    {
        private static readonly Lazy<IUserDAL> _lazyUserDAL = ObjectContainer.LazyResolve<IUserDAL>();
        private IUserDAL UserDAL => _lazyUserDAL.Value;

        public async Task<string> RemoveAddressAsync(RemoveAddressCommand cmd)
        {
            return await UserDAL.DeleteUserAddressAsync(cmd.Address).ConfigureAwait(false);
        }

        public async Task<string> SaveAddressAsync(SaveAddressCommand cmd)
        {
            // 格式化地址
            cmd.Address.Address1 = Utils.FormatAddress(cmd.Address.Address1);
            cmd.Address.Address2 = Utils.FormatAddress(cmd.Address.Address2);
            cmd.Address.City = Utils.FormatAddress(cmd.Address.City);
            cmd.Address.State = Utils.FormatAddress(cmd.Address.State);
            cmd.Address.ZipCode = Utils.FormatAddress(cmd.Address.ZipCode);
            cmd.Address.Country = "US";
            cmd.Address.Hash = cmd.Address.ToSha1();

            var existAddress = await UserDAL.GetUserAddressAsync(cmd.Address.User_ID, cmd.Address.Hash).ConfigureAwait(false);
            if (existAddress.IsNull())
            {// 没有才添加
                return await UserDAL.InsertUserAddressAsync(cmd.Address).ConfigureAwait(false);
            }
            else
            {// 更新
                return MsgCodes.AddressExists;
            }
        }
    }
}
