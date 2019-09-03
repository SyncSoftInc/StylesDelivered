using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.ShoppingCart
{
    public class ShoppingCartDF : IShoppingCartDF
    {
        private static readonly Lazy<IShoppingCartDAL> _lazyShoppingCartDAL = ObjectContainer.LazyResolve<IShoppingCartDAL>();
        private IShoppingCartDAL ShoppingCartDAL => _lazyShoppingCartDAL.Value;

        public Task<ShoppingCartItemDTO> GetCartItemAsync(Guid cartId, string itemNo)
        {
            return ShoppingCartDAL.GetItemAsync(cartId, itemNo);
        }
    }
}
