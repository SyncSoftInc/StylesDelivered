using SyncSoft.App;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Product
{
    public interface IProductService
    {
        Task<string> CreateItemAsync(ProductItemDTO dto);
        Task<string> UpdateItemAsync(ProductItemDTO dto);
        Task<string> DeleteItemAsync(string itemNo);
        Task<MsgResult<ProductItemDTO>> UploadImageAsync(UploadProductImageCommand cmd);
    }
}