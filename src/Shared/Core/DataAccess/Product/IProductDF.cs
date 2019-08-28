using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Product
{
    public interface IProductDF
    {
        Task<PagedList<ProductItemDTO>> GetProductItemsAsync(GetProductsQuery query);
        Task<ProductItemDTO> GetItemAsync(string itemNo);
    }
}
