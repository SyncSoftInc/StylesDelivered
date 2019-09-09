using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Product
{
    public interface IProductDAL
    {
        Task<string> InsertProductAsync(ProductDTO dto);
        Task<string> UpdateProductAsync(ProductDTO dto);
        Task<string> UpdateProductImageAsync(ProductDTO dto);
        Task<string> DeleteProductAsync(string asin);

        Task<ProductDTO> GetProductAsync(string asin);
        Task<PagedList<ProductDTO>> GetProductsAsync(GetProductsQuery query);
        Task<string> UpdateItemsJsonAsync(string asin, string json);

        //Task<string> SetItemInventoriesAsync(IDictionary<string, int> inventories);
    }
}
