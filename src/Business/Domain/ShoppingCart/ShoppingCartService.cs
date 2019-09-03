using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;
using SyncSoft.StylesDelivered.Enum.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IShoppingCartDAL> _lazyShoppingCartDAL = ObjectContainer.LazyResolve<IShoppingCartDAL>();
        private IShoppingCartDAL ShoppingCartDAL => _lazyShoppingCartDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  GetOrInit  -

        public async Task<ShoppingCartDTO> GetOrInitAsync(Guid userId)
        {
            var dto = await ShoppingCartDAL.GetCartAsync(userId).ConfigureAwait(false);
            if (dto.IsNull())
            {
                dto = new ShoppingCartDTO { ID = userId };
                var msgCode = await ShoppingCartDAL.InsertCartAsync(dto);
                if (!msgCode.IsSuccess())
                {
                    throw new Exception(MsgCodes.CreateShoppingCartFailed);
                }
            }

            return dto;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  AddItemAsync  -

        public async Task<string> AddItemAsync(AddItemCommand cmd)
        {
            var dto = await ShoppingCartDAL.GetItemAsync(cmd.Item.Cart_ID, cmd.Item.ItemNo).ConfigureAwait(false);
            if (dto.IsNull())
            {
                cmd.Item.Status = ShoppingCartItemStatusEnum.Active;
                cmd.Item.AddedOnUtc = DateTime.UtcNow;
                return await ShoppingCartDAL.InsertItemAsync(cmd.Item).ConfigureAwait(false);
            }
            else
            {
                dto.Qty += cmd.Item.Qty;
                return await ShoppingCartDAL.UpdateItemQtyAsync(dto).ConfigureAwait(false);
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  RemoveItem  -

        public Task<string> RemoveItemAsync(RemoveItemCommand cmd)
        {
            return ShoppingCartDAL.DeleteItemAsync(cmd.Item);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  ChangeItemQty  -

        public Task<string> ChangeItemQtyAsync(ChangeItemQtyCommand cmd)
        {
            return ShoppingCartDAL.UpdateItemQtyAsync(cmd.Item);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SaveForLater  -

        public Task<string> SaveForLaterAsync(SaveForLaterCommand cmd)
        {
            cmd.Item.Status = ShoppingCartItemStatusEnum.SavedForLater;
            return ShoppingCartDAL.UpdateItemStatusAsync(cmd.Item);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  ShopNow  -

        public Task<string> ShopNowAsync(SaveForLaterCommand cmd)
        {
            cmd.Item.Status = ShoppingCartItemStatusEnum.Active;
            return ShoppingCartDAL.UpdateItemStatusAsync(cmd.Item);
        }

        #endregion
    }
}
