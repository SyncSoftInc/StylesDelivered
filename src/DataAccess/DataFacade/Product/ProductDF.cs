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
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductDAL> _lazyProductDAL = ObjectContainer.LazyResolve<IProductDAL>();
        private IProductDAL ProductDAL => _lazyProductDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Get  -

        public async Task<ProductDTO> GetProductAsync(string asin)
        {
            var dto = await ProductDAL.GetProductAsync(asin).ConfigureAwait(false);
            return dto;
        }

        public async Task<PagedList<ProductDTO>> GetProductsAsync(GetProductsQuery query)
        {
            var rs = await ProductDAL.GetProductsAsync(query).ConfigureAwait(false);

            return rs;
        }

        #endregion
    }
}
