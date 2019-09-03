using SyncSoft.StylesDelivered.DTO.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.ShoppingCart
{
    public interface IShoppingCartDF
    {
        Task<ShoppingCartItemDTO> GetCartItemAsync(Guid cartId, string itemNo);
    }
}
