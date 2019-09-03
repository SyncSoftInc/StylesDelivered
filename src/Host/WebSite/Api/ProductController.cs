using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using SyncSoft.StylesDelivered.WebSite.Models;
using System;
using System.IO;
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
        #region -  CURD  -

        /// <summary>
        /// 创建Item
        /// </summary>
        [HttpPost("api/product/item")]
        public Task<string> CreateItemAsync(CreateProductItemCommand cmd)
            => base.RequestAsync(cmd);

        /// <summary>
        /// Upadte Item
        /// </summary>
        [HttpPut("api/product/item")]
        public Task<string> UpdateItemAsync(UpdateProductItemCommand cmd)
        {
            using (var stream = Request.Form.Files[0].OpenReadStream())
            {
                cmd.PictureData = stream.ToBytes();
            }

            return base.RequestAsync(cmd);
        }

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
        public Task<string> DeleteItemAsync(DeleteProductItemCommand cmd)
            => base.RequestAsync(cmd);

        /// <summary>
        /// Upadte Item
        /// </summary>
        [HttpPost("api/product/upload")]
        public Task<string> UploadImageAsync()
        {
            return Task.FromResult(MsgCodes.SUCCESS);
        }
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
                OrderBy = model.OrderBy,
                Draw = model.Draw,
                Keyword = model.Keyword,
                SortDirection = model.SortDirection,
            };
            query.SetContext(User.Identity);

            var plist = await ProductDF.GetProductItemsAsync(query).ConfigureAwait(false);
            return new DataTables<ProductItemDTO>(query.Draw, plist);
        }

        #endregion

    }
}