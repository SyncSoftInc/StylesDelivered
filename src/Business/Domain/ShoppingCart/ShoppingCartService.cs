//using SyncSoft.App.Components;
//using SyncSoft.StylesDelivered.Command.ShoppingCart;
//using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
//using SyncSoft.StylesDelivered.Domain.Inventory;
//using SyncSoft.StylesDelivered.DTO.ShoppingCart;
//using SyncSoft.StylesDelivered.Enum.ShoppingCart;
//using System;
//using System.Threading.Tasks;

//namespace SyncSoft.StylesDelivered.Domain.ShoppingCart
//{
//    public class ShoppingCartService : IShoppingCartService
//    {
//        // *******************************************************************************************************************************
//        #region -  Lazy Object(s)  -

//        private static readonly Lazy<IShoppingCartDAL> _lazyShoppingCartDAL = ObjectContainer.LazyResolve<IShoppingCartDAL>();
//        private IShoppingCartDAL ShoppingCartDAL => _lazyShoppingCartDAL.Value;

//        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
//        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

//        #endregion
//        // *******************************************************************************************************************************
//        #region -  GetOrInit  -

//        public async Task<ShoppingCartDTO> GetOrInitAsync(Guid userId)
//        {
//            var dto = await ShoppingCartDAL.GetCartAsync(userId).ConfigureAwait(false);
//            if (dto.IsNull())
//            {
//                dto = new ShoppingCartDTO { ID = userId };
//                var msgCode = await ShoppingCartDAL.InsertCartAsync(dto);
//                if (!msgCode.IsSuccess())
//                {
//                    throw new Exception(MsgCodes.CreateShoppingCartFailed);
//                }
//            }

//            return dto;
//        }

//        #endregion
//        // *******************************************************************************************************************************
//        #region -  AddItemAsync  -

//        public async Task<string> AddItemAsync(AddItemCommand cmd)
//        {
//            cmd.Item.Cart_ID = cmd.Identity.UserID();   // 防止跨购物车攻击

//            // 库存检查
//            var itemInventory = ItemInventoryFactory.Create(cmd.Item.ItemNo);
//            //var isAvailable = itemInventory.IsAvailable(cmd.Item.Qty);
//            //if (!isAvailable) return MsgCodes.ShortageOfInventory;
//            // ^^^^^^^^^^

//            var itemDTO = await ShoppingCartDAL.GetItemAsync(cmd.Item.Cart_ID, cmd.Item.ItemNo).ConfigureAwait(false);
//            if (itemDTO.IsNull())
//            {
//                cmd.Item.Status = ShoppingCartItemStatusEnum.Active;
//                cmd.Item.AddedOnUtc = DateTime.UtcNow;
//                return await ShoppingCartDAL.InsertItemAsync(cmd.Item).ConfigureAwait(false);
//            }
//            else
//            {
//                itemDTO.Qty += cmd.Item.Qty;
//                return await ShoppingCartDAL.UpdateItemQtyAsync(itemDTO).ConfigureAwait(false);
//            }
//        }

//        #endregion
//        // *******************************************************************************************************************************
//        #region -  RemoveItem  -

//        public Task<string> RemoveItemAsync(RemoveItemCommand cmd)
//        {
//            cmd.Item.Cart_ID = cmd.Identity.UserID();   // 防止跨购物车攻击
//            return ShoppingCartDAL.DeleteItemAsync(cmd.Item);
//        }

//        #endregion
//        // *******************************************************************************************************************************
//        #region -  ChangeItemQty  -

//        public Task<string> ChangeItemQtyAsync(ChangeItemQtyCommand cmd)
//        {
//            cmd.Item.Cart_ID = cmd.Identity.UserID();   // 防止跨购物车攻击
//            return ShoppingCartDAL.UpdateItemQtyAsync(cmd.Item);
//        }

//        #endregion
//        // *******************************************************************************************************************************
//        #region -  SaveForLater  -

//        public Task<string> SaveForLaterAsync(SaveForLaterCommand cmd)
//        {
//            cmd.Item.Cart_ID = cmd.Identity.UserID();   // 防止跨购物车攻击
//            cmd.Item.Status = ShoppingCartItemStatusEnum.SavedForLater;
//            return ShoppingCartDAL.UpdateItemStatusAsync(cmd.Item);
//        }

//        #endregion
//        // *******************************************************************************************************************************
//        #region -  ShopNow  -

//        public Task<string> ShopNowAsync(SaveForLaterCommand cmd)
//        {
//            cmd.Item.Cart_ID = cmd.Identity.UserID();   // 防止跨购物车攻击
//            cmd.Item.Status = ShoppingCartItemStatusEnum.Active;
//            return ShoppingCartDAL.UpdateItemStatusAsync(cmd.Item);
//        }

//        #endregion
//    }
//}
