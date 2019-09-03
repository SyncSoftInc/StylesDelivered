using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
using SyncSoft.StylesDelivered.Domain.ShoppingCart;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;
using SyncSoft.StylesDelivered.Enum.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace ShoppingCart
{
    public class Item
    {
        private static readonly Guid _userId = Guid.Empty;
        private const string _itemNo = "ITEM-001";

        private static readonly Lazy<IShoppingCartService> _lazyShoppingCartService = ObjectContainer.LazyResolve<IShoppingCartService>();
        private IShoppingCartService ShoppingCartService => _lazyShoppingCartService.Value;

        private static readonly Lazy<IShoppingCartDF> _lazyShoppingCartDF = ObjectContainer.LazyResolve<IShoppingCartDF>();
        private IShoppingCartDF ShoppingCartDF => _lazyShoppingCartDF.Value;

        [Test, Order(0)]
        public async Task AddItem()
        {
            var msgCode = await ShoppingCartService.AddItemAsync(new AddItemCommand
            {
                Item = new ShoppingCartItemDTO
                {
                    Cart_ID = _userId,
                    ItemNo = _itemNo,
                    Qty = 12,
                }
            }).ConfigureAwait(false);

            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        [Test, Order(10)]
        public async Task ChangeItemQty()
        {
            var msgCode = await ShoppingCartService.ChangeItemQtyAsync(new ChangeItemQtyCommand
            {
                Item = new ShoppingCartItemDTO
                {
                    Cart_ID = _userId,
                    ItemNo = _itemNo,
                    Qty = 5,
                }
            }).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            var dto = await ShoppingCartDF.GetCartItemAsync(_userId, _itemNo).ConfigureAwait(false);
            Assert.AreEqual(dto.Qty, 5);
        }

        [Test, Order(20)]
        public async Task SaveForLater()
        {
            var msgCode = await ShoppingCartService.SaveForLaterAsync(new SaveForLaterCommand
            {
                Item = new ShoppingCartItemDTO
                {
                    Cart_ID = _userId,
                    ItemNo = _itemNo,
                }
            }).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            var dto = await ShoppingCartDF.GetCartItemAsync(_userId, _itemNo).ConfigureAwait(false);
            Assert.AreEqual(dto.Status, ShoppingCartItemStatusEnum.SavedForLater);
        }

        [Test, Order(30)]
        public async Task ShowNow()
        {
            var msgCode = await ShoppingCartService.ShopNowAsync(new SaveForLaterCommand
            {
                Item = new ShoppingCartItemDTO
                {
                    Cart_ID = _userId,
                    ItemNo = _itemNo,
                }
            }).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            var dto = await ShoppingCartDF.GetCartItemAsync(_userId, _itemNo).ConfigureAwait(false);
            Assert.AreEqual(dto.Status, ShoppingCartItemStatusEnum.Active);
        }

        [Test, Order(40)]
        public async Task RemoveItem()
        {
            var msgCode = await ShoppingCartService.RemoveItemAsync(new RemoveItemCommand
            {
                Item = new ShoppingCartItemDTO
                {
                    Cart_ID = _userId,
                    ItemNo = _itemNo,
                }
            }).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            var dto = await ShoppingCartDF.GetCartItemAsync(_userId, _itemNo).ConfigureAwait(false);
            Assert.IsNull(dto);
        }
    }
}