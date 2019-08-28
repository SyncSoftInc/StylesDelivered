using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Product
{
    public interface IProductDAL
    {
        Task<string> InsertItemAsync(ProductItemDTO dto);

        Task<string> DeleteProductItemAsync(string itemNo);

        Task<PagedList<ProductItemDTO>> GetProductItemsAsync(GetProductsQuery query);
        Task<ProductItemDTO> GetProductItemAsync(string itemNo);
    }
}
