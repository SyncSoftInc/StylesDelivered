using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
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

        private const string _imageRoot = "https://eec.oss-us-west-1.aliyuncs.com/";

        #endregion
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductDAL> _lazyProductDAL = ObjectContainer.LazyResolve<IProductDAL>();
        private IProductDAL ProductDAL => _lazyProductDAL.Value;

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        private static readonly Lazy<IStorage> _lazyStorage = ObjectContainer.LazyResolve<IStorage>();
        private IStorage Storage => _lazyStorage.Value;

        private static readonly Lazy<IMessageDispatcher> _lazyMessageDispatcher = ObjectContainer.LazyResolve<IMessageDispatcher>();
        private IMessageDispatcher MessageDispatcher => _lazyMessageDispatcher.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public async Task<string> CreateProductAsync(ProductDTO dto)
        {
            var msgCode = CheckProductDTO(dto);
            if (!msgCode.IsSuccess()) return msgCode;
            // ^^^^^^^^^^

            var exists = await ProductDAL.GetProductAsync(dto.ASIN).ConfigureAwait(false);
            if (exists.IsNotNull())
            {
                return "ASIN already exists.";
            }

            dto.CreatedOnUtc = DateTime.UtcNow;
            return await ProductDAL.InsertProductAsync(dto).ConfigureAwait(false);
        }

        public async Task<string> UpdateProductAsync(ProductDTO dto)
        {
            var msgCode = CheckProductDTO(dto);
            if (!msgCode.IsSuccess()) return msgCode;

            var product = await ProductDAL.GetProductAsync(dto.ASIN).ConfigureAwait(false);
            if (product.IsNull()) return MsgCodes.ProductNotExists;
            // ^^^^^^^^^^

            return await ProductDAL.UpdateProductAsync(dto).ConfigureAwait(false);
        }

        public async Task<string> DeleteProductAsync(string asin)
        {
            return await ProductDAL.DeleteProductAsync(asin).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  UploadImageAsync  -

        public async Task<MsgResult<ProductDTO>> UploadImageAsync(UploadProductImageCommand cmd)
        {
            var dto = await ProductDAL.GetProductAsync(cmd.asin).ConfigureAwait(false);
            if (dto.IsNotNull())
            {
                var sha1 = cmd.PictureData.ToSha1String();
                var key = $"p/{ DateTime.Now:yyyy'/'MM'/'dd}/{sha1}.jpg";
                var msgCode = await Storage.SaveAsync(key, cmd.PictureData).ConfigureAwait(false);
                if (!msgCode.IsSuccess()) return new MsgResult<ProductDTO>(MsgCodes.SaveFileToCloudFailed);

                dto.ImageUrl = _imageRoot + key;
                await ProductDAL.UpdateProductImageAsync(dto).ConfigureAwait(false);
            }

            return new MsgResult<ProductDTO>(dto);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Utilities  -

        private string CheckProductDTO(ProductDTO dto)
        {
            if (dto.ASIN.IsNull()) return MsgCodes.ASINCannotBeEmpty;
            if (dto.ProductName.IsNull()) return MsgCodes.ProductNameCannotBeEmpty;

            if (dto.ASIN.IsNotNull() && dto.ASIN.Length > 20) return MsgCodes.InvalidASINLength;
            if (dto.ProductName.IsNotNull() && dto.ProductName.Length > 200) return MsgCodes.InvalidProductNameLength;
            if (dto.Description.IsNotNull() && dto.Description.Length > 2000) return MsgCodes.InvalidDescriptionLength;
            if (dto.ImageUrl.IsNotNull() && dto.ImageUrl.Length > 200) return MsgCodes.InvalidImageUrlLength;

            return MsgCodes.SUCCESS;
        }

        #endregion
    }
}
