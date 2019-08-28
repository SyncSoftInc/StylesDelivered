using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Product;
using SyncSoft.StylesDelivered.DTO.Product;
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

        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CURD  -

        /// <summary>
        /// 创建Item
        /// </summary>
        [HttpPost("api/product/item")]
        public async Task<string> CreateItemAsync(ProductItemDTO dto)
            => await ProductService.CreateItemAsync(dto);

        /// <summary>
        /// 获取Item
        /// </summary>
        [HttpGet("api/product/item/{itemNo}")]
        public Task<ProductItemDTO> GetItemAsync(string itemNo)
            => ProductDF.GetItemAsync(itemNo);

        /// <summary>
        /// 删除Item
        /// </summary>
        [HttpDelete("api/product/item/{itemNo}")]
        public Task<string> DeleteItemAsync(string itemNo)
            => ProductService.DeleteItemAsync(itemNo);

        #endregion
        // *******************************************************************************************************************************
        #region -  GetProducts  -
        /// <summary>
        /// 获取分页Item数据
        /// </summary>
        [HttpGet("api/product/items")]
        public async Task<DataTables<ProductItemDTO>> GetProductsAsync(DataTableModel model)
        {
            var query = new GetProductsQuery
            {
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                Draw = model.Draw,
                Keyword = model.Keyword
            };
            query.SetContext(User.Identity);

            var plist = await ProductDF.GetProductItemsAsync(query).ConfigureAwait(false);
            return new DataTables<ProductItemDTO>(query.Draw, plist);
        }

        #endregion
    }
}