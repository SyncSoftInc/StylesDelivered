﻿using Microsoft.AspNetCore.Mvc;
using SyncSoft.App;
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
    public class AdminProductController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductDF> _lazyProductDF = ObjectContainer.LazyResolve<IProductDF>();
        private IProductDF ProductDF => _lazyProductDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CURD  -

        /// <summary>
        /// 创建Product
        /// </summary>
        [HttpPost("api/admin/product")]
        public Task<string> CreateItemAsync(CreateProductCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// Upadte Product
        /// </summary>
        [HttpPut("api/admin/product")]
        public Task<string> UpdateItemAsync(UpdateProductCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 删除Product
        /// </summary>
        [HttpDelete("api/admin/product/{asin}")]
        public Task<string> DeleteItemAsync(DeleteProductCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 获取Product
        /// </summary>
        [HttpGet("api/admin/product/{asin}")]
        public Task<ProductDTO> GetItemAsync(string asin) => ProductDF.GetProductAsync(asin);

        /// <summary>
        /// Upadte Product Image
        /// </summary>
        [HttpPost("api/admin/product/upload")]
        public Task<MsgResult<ProductDTO>> UploadImageAsync(UploadProductImageCommand cmd)
        {
            using (var stream = Request.Form.Files[0].OpenReadStream())
            {
                var asin = Request.Form["PostData[ASIN]"].ToString();
                if (asin.IsNotNull())
                {
                    cmd.asin = asin;
                    cmd.PictureData = stream.ToBytes();
                }
            }

            return base.RequestAsync<UploadProductImageCommand, MsgResult<ProductDTO>>(cmd);
        }

        /// <summary>
        /// Upadte Product Status
        /// </summary>
        [HttpPatch("api/admin/product")]
        public Task<string> UpdateStatusAsync(UpdateProductStatusCommand cmd) => base.RequestAsync(cmd);

        #endregion
        // *******************************************************************************************************************************
        #region -  GetProducts  -

        /// <summary>
        /// 获取分页Product数据
        /// </summary>
        [HttpGet("api/admin/products")]
        public async Task<DataTables<ProductDTO>> GetProductsAsync(ProductQueryModel model)
        {
            var query = new GetProductsQuery
            {
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                OrderBy = model.OrderBy,
                Draw = model.Draw,
                Keyword = model.Keyword,
                SortDirection = model.SortDirection,
                Status = model.Status
            };
            query.SetContext(User.Identity);

            var plist = await ProductDF.GetProductsAsync(query).ConfigureAwait(false);
            return new DataTables<ProductDTO>(query.Draw, plist);
        }

        #endregion
    }
}