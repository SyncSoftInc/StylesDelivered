using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Query.Order;
using SyncSoft.StylesDelivered.WebSite.Models;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class AdminOrderItemController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderItemDF> _lazyOrderItemDF = ObjectContainer.LazyResolve<IOrderItemDF>();
        private IOrderItemDF OrderItemDF => _lazyOrderItemDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  GetItems  -

        /// <summary>
        /// 获取分页OrderItem数据
        /// </summary>
        [HttpGet("api/admin/order/items")]
        public async Task<DataTables<OrderItemDTO>> GetItemsAsync(OrderItemQueryModel model)
        {
            var query = new GetOrderItemsQuery
            {
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                OrderBy = model.OrderBy,
                Draw = model.Draw,
                Keyword = model.Keyword,
                SortDirection = model.SortDirection,
                OrderNo = model.OrderNo
            };
            query.SetContext(User.Identity);

            var plist = await OrderItemDF.GetOrderItemsAsync(query).ConfigureAwait(false);
            return new DataTables<OrderItemDTO>(query.Draw, plist);
        }

        #endregion
    }
}