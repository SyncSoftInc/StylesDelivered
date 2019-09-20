using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECOM.DTOs;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.User;
using SyncSoft.StylesDelivered.DataAccess.User;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class AdminUserController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IUserDF> _lazyUserDF = ObjectContainer.LazyResolve<IUserDF>();
        private IUserDF UserDF => _lazyUserDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  AdminUser  -

        /// <summary>
        /// 创建用户
        /// </summary>
        [HttpPost("api/admin/user")]
        public Task<string> CreateUserAsync(CreateAdminUserCommand cmd)
        {
            return base.RequestAsync(cmd);
        }

        /// <summary>
        /// 保存用户基础信息
        /// </summary>
        [HttpPut("api/admin/user")]
        public Task<string> UpdateUserAsync(SaveAdminUserCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpDelete("api/admin/user/{id}")]
        public Task<string> DeleteUserAsync(DeleteAdminUserCommand cmd) => base.RequestAsync(cmd);

        #endregion
        // *******************************************************************************************************************************
        #region -  Queries  -

        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        [HttpGet("api/admin/user/{id}")]
        public Task<UserDTO> GetUserAsync(Guid id) => UserDF.GetUserAsync(id);

        ///// <summary>
        ///// 获取分页用户数据
        ///// </summary>
        //[HttpGet("api/admin/users")]
        //public async Task<DataTables<UserDTO>> GetUsersAsync(DataTableModel model)
        //{
        //    var query = new GetUsersQuery
        //    {
        //        PageSize = model.PageSize,
        //        PageIndex = model.PageIndex,
        //        OrderBy = model.OrderBy,
        //        Draw = model.Draw,
        //        Keyword = model.Keyword,
        //        SortDirection = model.SortDirection,
        //    };
        //    query.SetContext(User.Identity);

        //    var plist = await UserDF.GetUsersAsync(query).ConfigureAwait(false);
        //    return new DataTables<UserDTO>(query.Draw, plist);
        //}

        #endregion
    }
}