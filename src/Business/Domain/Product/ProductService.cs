using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.App.Securities;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DataAccess.Inventory;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Event.Inventory;
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

        private static readonly Lazy<IStorage> _lazyStorage = ObjectContainer.LazyResolve<IStorage>();
        private IStorage Storage => _lazyStorage.Value;

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
            var msgCode = CheckItemDTO(dto);
            if (!msgCode.IsSuccess()) return msgCode;
            // ^^^^^^^^^^

            msgCode = await ProductDAL.UpdateItemAsync(dto).ConfigureAwait(false);

            if (msgCode.IsSuccess())
            {
                // 抛出库存更改事件
                _ = MessageDispatcher.PublishAsync(new ItemInventoryChangedEvent(dto.ItemNo, dto.InvQty));
            }

            return msgCode;
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

                dto.ImageUrl = _imageRoot + key;
                await ProductDAL.UpdateItemImageAsync(dto).ConfigureAwait(false);
            }

            return new MsgResult<ProductItemDTO>(dto);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SyncInventoriesAsync  -

        public async Task<string> SyncInventoriesAsync()
        {
            var inventories = await InventoryDAL.GetItemInventoriesAsync().ConfigureAwait(false);
            return await ProductDAL.SetItemInventoriesAsync(inventories).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Utilities  -

        private string CheckItemDTO(ProductItemDTO dto)
        {
            if (dto.ItemNo.IsNull()) return MsgCodes.ItemNoCannotBeEmpty;
            if (dto.ProductName.IsNull()) return MsgCodes.ProductNameCannotBeEmpty;

            if (dto.ItemNo.IsNotNull() && dto.ItemNo.Length > 20) return MsgCodes.InvalidItemNoLength;
            if (dto.ProductName.IsNotNull() && dto.ProductName.Length > 50) return MsgCodes.InvalidProductNameLength;
            if (dto.Description.IsNotNull() && dto.Description.Length > 1000) return MsgCodes.InvalidDescriptionLength;
            if (dto.ImageUrl.IsNotNull() && dto.ImageUrl.Length > 200) return MsgCodes.InvalidImageUrlLength;

            if (dto.InvQty < 0) return MsgCodes.InvalidInventoryQuantity;

            return MsgCodes.SUCCESS;
        }

        #endregion
    }
}
