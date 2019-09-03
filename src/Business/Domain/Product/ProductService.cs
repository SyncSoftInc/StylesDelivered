using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Securities;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Storage;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Product
{
    public class ProductService : IProductService
    {
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        private const string _urlRoot = "https://eec.oss-us-west-1.aliyuncs.com/";

        #endregion
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductDAL> _lazyProductDAL = ObjectContainer.LazyResolve<IProductDAL>();
        private IProductDAL ProductDAL => _lazyProductDAL.Value;

        private static readonly Lazy<IStorage> _lazyStorage = ObjectContainer.LazyResolve<IStorage>();
        private IStorage Storage => _lazyStorage.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public async Task<string> CreateItemAsync(ProductItemDTO dto)
        {
            var existsItem = await ProductDAL.GetProductItemAsync(dto.ItemNo).ConfigureAwait(false);
            if (existsItem.IsNotNull())
            {
                return "ItemNo already exists.";
            }

            dto.CreatedOnUtc = DateTime.UtcNow;
            return await ProductDAL.InsertItemAsync(dto).ConfigureAwait(false);
        }

        public async Task<string> UpdateItemAsync(ProductItemDTO dto)
        {
            return await ProductDAL.UpdateItemAsync(dto).ConfigureAwait(false);
        }

        public async Task<string> DeleteItemAsync(string itemNo)
        {
            return await ProductDAL.DeleteProductItemAsync(itemNo).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  UploadImageAsync  -

        public async Task<MsgResult<ProductItemDTO>> UploadImageAsync(UploadProductImageCommand cmd)
        {
            var dto = await ProductDAL.GetProductItemAsync(cmd.ItemNo).ConfigureAwait(false);
            if (dto.IsNotNull())
            {
                var sha1 = cmd.PictureData.ToSha1String();
                var key = $"p/{ DateTime.Now:yyyy'/'MM'/'dd}/{sha1}.jpg";
                var msgCode = await Storage.SaveAsync(key, cmd.PictureData).ConfigureAwait(false);
                if (!msgCode.IsSuccess()) return new MsgResult<ProductItemDTO>(MsgCodes.SaveFileToCloudFailed);

                dto.ImageUrl = _urlRoot + key;
                await ProductDAL.UpdateItemImageAsync(dto).ConfigureAwait(false);
            }

            return new MsgResult<ProductItemDTO>(dto);
        }

        #endregion
    }
}
