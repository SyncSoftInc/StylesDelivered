using SyncSoft.App.Components;
using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Inventory;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.Product
{
    public class ProductItemDF : IProductItemDF
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Get  -

        public async Task<ProductItemDTO> GetItemAsync(string asin, string sku)
        {
            var dto = await ProductItemDAL.GetItemAsync(asin, sku).ConfigureAwait(false);
            //FillInvQty(dto);
            return dto;
        }

        public async Task<PagedList<ProductItemDTO>> GetItemsAsync(GetProductItemsQuery query)
        {
            var rs = await ProductItemDAL.GetItemsAsync(query).ConfigureAwait(false);
            return rs;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  // FillInvQty  -

        //private void FillInvQty(ProductItemDTO dto)
        //{
        //    if (dto.IsNotNull())
        //    {
        //        var inventory = ItemInventoryFactory.Create(dto.ItemNo);
        //        dto.InvQty = inventory.Get();
        //    }
        //}

        #endregion
    }
}
