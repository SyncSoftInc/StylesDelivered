using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Order
{
    public interface IOrderDF
    {
        Task<OrderDTO> GetOrderAsync(string orderNo);
        Task<PagedList<OrderDTO>> GetOrdersAsync(GetOrdersQuery query);
    }
}
