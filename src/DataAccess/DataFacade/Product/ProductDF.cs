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
    public class ProductDF : IProductDF
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductDAL> _lazyProductDAL = ObjectContainer.LazyResolve<IProductDAL>();
        private IProductDAL ProductDAL => _lazyProductDAL.Value;

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        #endregion

        public async Task<ProductItemDTO> GetItemAsync(string itemNo)
        {
            var dto = await ProductDAL.GetProductItemAsync(itemNo).ConfigureAwait(false);
            FillInvQty(dto);
            return dto;
        }

        public async Task<PagedList<ProductItemDTO>> GetProductItemsAsync(GetProductsQuery query)
        {
            var rs = await ProductDAL.GetProductItemsAsync(query).ConfigureAwait(false);
            return rs;
        }
        // *******************************************************************************************************************************
        #region -  FillInvQty  -

        private void FillInvQty(ProductItemDTO dto)
        {
            if (dto.IsNotNull())
            {
                var inventory = ItemInventoryFactory.Create(dto.ItemNo);
                dto.InvQty = inventory.Get();
            }
        }

        #endregion
    }
}
