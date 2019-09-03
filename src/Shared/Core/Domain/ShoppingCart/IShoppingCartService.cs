using SyncSoft.StylesDelivered.Command.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.ShoppingCart
{
    public interface IShoppingCartService
    {
        Task<string> AddItemAsync(AddItemCommand cmd);
        Task<string> RemoveItemAsync(RemoveItemCommand cmd);
        Task<string> ChangeItemQtyAsync(ChangeItemQtyCommand cmd);
    }
}
