using SyncSoft.App.Components;
using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.Product
{
    public class ProductDF : IProductDF
    {
        private static readonly Lazy<IProductDAL> _lazyProductDAL = ObjectContainer.LazyResolve<IProductDAL>();
        private IProductDAL ProductDAL => _lazyProductDAL.Value;

        public Task<ProductItemDTO> GetItemAsync(string itemNo)
            => ProductDAL.GetProductItemAsync(itemNo);

        public Task<PagedList<ProductItemDTO>> GetProductItemsAsync(GetProductsQuery query)
            => ProductDAL.GetProductItemsAsync(query);
    }
}
