using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Product
{
    public interface IProductItemDF
    {
        Task<ProductItemDTO> GetItemAsync(string asin, string sku);
        Task<PagedList<ProductItemDTO>> GetItemsAsync(GetProductItemsQuery query);
    }
}
