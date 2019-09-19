using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Query.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Product
{
    public interface IProductItemDAL
    {
        Task<string> InsertItemAsync(ProductItemDTO dto);
        Task<string> UpdateItemAsync(ProductItemDTO dto);
        Task<string> DeleteItemAsync(string asin, string sku);

        Task<ProductItemDTO> GetItemAsync(string asin, string sku);
        Task<PagedList<ProductItemDTO>> GetItemsAsync(GetProductItemsQuery query);
        Task<IList<ProductItemDTO>> GetItemsAsync(string productASIN);
        Task<string> SetItemInvQtysdAsync(IDictionary<string, long> inventories);
        Task<string> SetItemHoldInvQtysdAsync(IDictionary<string, long> inventories);
    }
}
