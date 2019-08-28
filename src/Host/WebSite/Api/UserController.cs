using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.User;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.Domain.User;
using SyncSoft.StylesDelivered.DTO.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class UserController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IUserDF> _lazyUserDF = ObjectContainer.LazyResolve<IUserDF>();
        private IUserDF UserDF => _lazyUserDF.Value;

        private static readonly Lazy<IUserService> _lazyUserService = ObjectContainer.LazyResolve<IUserService>();
        private IUserService UserService => _lazyUserService.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CURD  -

        /// <summary>
        /// 创建地址
        /// </summary>
        [HttpPost("api/user/address")]
        public Task<string> CreateAddressAsync(SaveAddressCommand cmd)
            => UserService.SaveAddressAsync(cmd);

        /// <summary>
        /// 删除地址
        /// </summary>
        [HttpDelete("api/user/address")]
        public Task<string> DeleteAddressAsync(RemoveAddressCommand cmd)
            => UserService.RemoveAddressAsync(cmd);

        #endregion
        // *******************************************************************************************************************************
        #region -  GetAddresses  -

        /// <summary>
        /// 获取用户地址
        /// </summary>
        [HttpGet("api/user/addresses")]
        public async Task<IList<AddressDTO>> GetAddressesAsync()
        {
            var list = await UserDF.GetUserAddressesAsync(User.Identity.UserID()).ConfigureAwait(false);
            return list;
        }

        #endregion
    }
}