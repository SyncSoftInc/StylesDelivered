using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Query.Order;
using SyncSoft.StylesDelivered.WebSite.Models;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class AdminOrderController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderDF> _lazyOrderDF = ObjectContainer.LazyResolve<IOrderDF>();
        private IOrderDF OrderDF => _lazyOrderDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        /// <summary>
        /// Approve Order
        /// </summary>
        [HttpPatch("api/admin/order/{orderNo}")]
        public Task<string> ApproveOrderAsync(ApproveOrderCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 删除Order
        /// </summary>
        [HttpDelete("api/admin/order/{orderNo}")]
        public Task<string> DeleteOrderAsync(DeleteOrderCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 获取Order
        /// </summary>
        [HttpGet("api/admin/order/{orderNo}")]
        public Task<OrderDTO> GetOrderAsync(string orderNo) => OrderDF.GetOrderAsync(orderNo);

        #endregion
        // *******************************************************************************************************************************
        #region -  GetOrders  -

        /// <summary>
        /// 获取分页Order数据
        /// </summary>
        [HttpGet("api/admin/orders")]
        public async Task<DataTables<OrderDTO>> GetOrdersAsync(DataTableModel model)
        {
            var query = new GetOrdersQuery
            {
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                OrderBy = model.OrderBy,
                Draw = model.Draw,
                Keyword = model.Keyword,
                SortDirection = model.SortDirection
            };
            query.SetContext(User.Identity);

            var plist = await OrderDF.GetOrdersAsync(query).ConfigureAwait(false);
            return new DataTables<OrderDTO>(query.Draw, plist);
        }

        #endregion
    }
}
