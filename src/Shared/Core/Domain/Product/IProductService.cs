using SyncSoft.StylesDelivered.DTO.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Product
{
    public interface IProductService
    {
        Task<string> CreateItemAsync(ProductItemDTO dto);
        Task<string> DeleteItemAsync(string itemNo);
    }
}