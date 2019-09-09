using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DataAccess.Inventory;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Event.Inventory;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Product
{
    public class ProductItemService : IProductItemService
    {
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        //private const string _imageRoot = "https://eec.oss-us-west-1.aliyuncs.com/";

        #endregion
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        private static readonly Lazy<IInventoryDAL> _lazyInventoryDAL = ObjectContainer.LazyResolve<IInventoryDAL>();
        private IInventoryDAL InventoryDAL => _lazyInventoryDAL.Value;

        private static readonly Lazy<IMessageDispatcher> _lazyMessageDispatcher = ObjectContainer.LazyResolve<IMessageDispatcher>();
        private IMessageDispatcher MessageDispatcher => _lazyMessageDispatcher.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        public async Task<string> CreateItemAsync(ProductItemDTO dto)
        {
            var msgCode = CheckItemDTO(dto);
            if (!msgCode.IsSuccess()) return msgCode;
            // ^^^^^^^^^^

            var existsItem = await ProductItemDAL.GetItemAsync(dto.ASIN, dto.SKU).ConfigureAwait(false);
            if (existsItem.IsNotNull())
            {
                return "Item already exists.";
            }

            return await ProductItemDAL.InsertItemAsync(dto).ConfigureAwait(false);
        }

        public async Task<string> UpdateItemAsync(ProductItemDTO dto)
        {
            var msgCode = CheckItemDTO(dto);
            if (!msgCode.IsSuccess()) return msgCode;
            // ^^^^^^^^^^

            msgCode = await ProductItemDAL.UpdateItemAsync(dto).ConfigureAwait(false);
            if (msgCode.IsSuccess())
            {
                // 抛出库存更改事件
                _ = MessageDispatcher.PublishAsync(new ItemInventoryChangedEvent(dto.SKU, dto.InvQty));
            }

            return msgCode;
        }

        public async Task<string> DeleteItemAsync(string asin, string sku)
        {
            return await ProductItemDAL.DeleteItemAsync(asin, sku).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SyncInventoriesAsync  -

        public async Task<string> SyncInventoriesAsync()
        {
            var inventories = await InventoryDAL.GetItemInventoriesAsync().ConfigureAwait(false);
            return await ProductItemDAL.SetItemInventoriesAsync(inventories).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Utilities  -

        private string CheckItemDTO(ProductItemDTO dto)
        {
            if (dto.ASIN.IsNull()) return MsgCodes.ASINCannotBeEmpty;
            if (dto.SKU.IsNull()) return MsgCodes.SKUCannotBeEmpty;

            if (dto.ASIN.IsNotNull() && dto.ASIN.Length > 20) return MsgCodes.InvalidASINLength;
            if (dto.SKU.IsNotNull() && dto.SKU.Length > 20) return MsgCodes.InvalidSKULength;
            if (dto.Alias.IsNotNull() && dto.Alias.Length > 200) return MsgCodes.InvalidAliasLength;
            if (dto.Color.IsNotNull() && dto.Color.Length > 30) return MsgCodes.InvalidColorLength;
            if (dto.Size.IsNotNull() && dto.Size.Length > 30) return MsgCodes.InvalidSizeLength;
            if (dto.Url.IsNotNull() && dto.Url.Length > 200) return MsgCodes.InvalidImageUrlLength;

            if (dto.InvQty < 0) return MsgCodes.InvalidInventoryQuantity;

            return MsgCodes.SUCCESS;
        }

        #endregion
    }
}
