using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Product
{
    public class ProductService : IProductService
    {
        private static readonly Lazy<IProductDAL> _lazyProductDAL = ObjectContainer.LazyResolve<IProductDAL>();
        private IProductDAL ProductDAL => _lazyProductDAL.Value;

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

        public async Task<string> DeleteItemAsync(string itemNo)
        {
            return await ProductDAL.DeleteProductItemAsync(itemNo).ConfigureAwait(false);
        }
    }
}
