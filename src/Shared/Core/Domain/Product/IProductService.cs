using SyncSoft.App;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Product
{
    public interface IProductService
    {
        Task<string> CreateProductAsync(ProductDTO dto);
        Task<string> UpdateProductAsync(ProductDTO dto);
        Task<string> DeleteProductAsync(string asin);
        Task<MsgResult<ProductDTO>> UploadImageAsync(UploadProductImageCommand cmd);
        Task<string> RefreshItemsJsonAsync(string asin);
    }
}