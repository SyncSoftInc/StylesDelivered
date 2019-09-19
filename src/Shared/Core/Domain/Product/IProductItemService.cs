using SyncSoft.StylesDelivered.DTO.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Product
{
    public interface IProductItemService
    {
        Task<string> CreateItemAsync(ProductItemDTO dto);
        Task<string> UpdateItemAsync(ProductItemDTO dto);
        Task<string> DeleteItemAsync(string asin, string sku);
        Task<string> SyncInventoriesAsync();
        Task<string> SyncHoldInventoriesAsync();

    }
}