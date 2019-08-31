using SyncSoft.App.Components;
using SyncSoft.App.Securities;
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

            return await ProductDAL.InsertItemAsync(dto).ConfigureAwait(false);
        }

        public async Task<string> UpdateItemAsync(ProductItemDTO dto)
        {
            await UploadImageAsync(dto.ImageUrl).ConfigureAwait(false);
            dto.ImageUrl = $"p/{DateTime.UtcNow:yyyy'/'MM'/'dd}/{dto.ImageUrl}";
            dto.CreatedOnUtc = DateTime.UtcNow;

            return await ProductDAL.UpdateItemAsync(dto).ConfigureAwait(false);
        }

        public async Task<string> DeleteItemAsync(string itemNo)
        {
            return await ProductDAL.DeleteProductItemAsync(itemNo).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Utilities  -

        private async Task<string> UploadImageAsync(string cmd)
        {
            var sha1 = cmd.Data.ToSha1String();

            await Storage.SaveAsync($"p/{DateTime.Now:yyyy'/'mm'/'dd}/{sha1}.jpg", cmd.Data).ConfigureAwait(false);
        }

        #endregion
    }
}
