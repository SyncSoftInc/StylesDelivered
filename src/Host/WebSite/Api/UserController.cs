using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.ECP.DTOs.Users;
using SyncSoft.StylesDelivered.Command.User;
using SyncSoft.StylesDelivered.DataAccess.User;
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

        #endregion
        // *******************************************************************************************************************************
        #region -  GetUser  -

        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        [HttpGet("api/user/{id}")]
        public Task<UserBasicInfoDTO> GetUserAsync(Guid id)
        {
            return UserDF.GetUserBasicInfoAsync(id);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Address  -

        /// <summary>
        /// 创建地址
        /// </summary>
        [HttpPost("api/user/address")]
        public Task<string> CreateAddressAsync(SaveAddressCommand cmd)
        {
            cmd.Address.User_ID = User.Identity.UserID();
            return base.RequestAsync(cmd);
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        [HttpDelete("api/user/address")]
        public Task<string> DeleteAddressAsync(RemoveAddressCommand cmd)
        {
            cmd.Address.User_ID = User.Identity.UserID();
            return base.RequestAsync(cmd);
        }

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