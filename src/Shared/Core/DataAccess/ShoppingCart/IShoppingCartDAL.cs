using SyncSoft.StylesDelivered.DTO.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.ShoppingCart
{
    public interface IShoppingCartDAL
    {
        Task<ShoppingCartDTO> GetCartAsync(Guid userId);
        Task<string> InsertCartAsync(ShoppingCartDTO dto);
        Task<string> InsertItemAsync(ShoppingCartItemDTO dto);
        Task<ShoppingCartItemDTO> GetItemAsync(Guid cartId, string itemNo);
        Task<string> DeleteItemAsync(ShoppingCartItemDTO dto);
        Task<string> UpdateItemQtyAsync(ShoppingCartItemDTO dto);
        Task<string> UpdateItemStatusAsync(ShoppingCartItemDTO dto);
    }
}
