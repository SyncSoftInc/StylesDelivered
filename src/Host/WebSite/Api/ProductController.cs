using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Enum.Product;
using SyncSoft.StylesDelivered.Query.Product;
using SyncSoft.StylesDelivered.WebSite.Models;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class ProductController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductDF> _lazyProductDF = ObjectContainer.LazyResolve<IProductDF>();
        private IProductDF ProductDF => _lazyProductDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  GetProducts  -

        /// <summary>
        /// 获取分页Product数据
        /// </summary>
        [HttpGet("api/products")]
        public async Task<DataTables<ProductDTO>> GetProductsAsync(DataTableModel model)
        {
            var query = new GetProductsQuery
            {
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                OrderBy = model.OrderBy,
                Draw = model.Draw,
                Keyword = model.Keyword,
                SortDirection = model.SortDirection,
                Status = ProductStatusEnum.Active
            };
            query.SetContext(User.Identity);

            var plist = await ProductDF.GetProductsAsync(query).ConfigureAwait(false);
            return new DataTables<ProductDTO>(query.Draw, plist);
        }

        #endregion
    }
}