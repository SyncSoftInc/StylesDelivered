using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using SyncSoft.StylesDelivered.WebSite.Models;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class ProductItemController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductItemDF> _lazyProductItemDF = ObjectContainer.LazyResolve<IProductItemDF>();
        private IProductItemDF ProductItemDF => _lazyProductItemDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CURD  -

        /// <summary>
        /// 创建Item
        /// </summary>
        [HttpPost("api/product/item")]
        public Task<string> CreateItemAsync(CreateItemCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// Upadte Item
        /// </summary>
        [HttpPut("api/product/item")]
        public Task<string> UpdateItemAsync(UpdateItemCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 删除Item
        /// </summary>
        [HttpDelete("api/product/item")]
        public Task<string> DeleteItemAsync(DeleteItemCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 获取Item
        /// </summary>
        [HttpGet("api/product/item")]
        public Task<ProductItemDTO> GetItemAsync(string asin, string sku) => ProductItemDF.GetItemAsync(asin, sku);

        #endregion
        // *******************************************************************************************************************************
        #region -  GetItems  -

        /// <summary>
        /// 获取分页Item数据
        /// </summary>
        [HttpGet("api/product/items")]
        public async Task<DataTables<ProductItemDTO>> GetItemsAsync(DataTableModel model, string asin)
        {
            var query = new GetProductItemsQuery
            {
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                OrderBy = model.OrderBy,
                Draw = model.Draw,
                Keyword = model.Keyword,
                ASIN = asin,
                SortDirection = model.SortDirection,
            };
            query.SetContext(User.Identity);

            var plist = await ProductItemDF.GetItemsAsync(query).ConfigureAwait(false);
            return new DataTables<ProductItemDTO>(query.Draw, plist);
        }

        #endregion

    }
}