using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Order
{
    public interface IOrderDAL
    {
        Task<string> InsertAsync(OrderDTO dto);
        Task<string> UpdateOrderStatusAsync(string orderNo, OrderStatusEnum status);
        Task<string> DeleteOrderAsync(string orderNo);

        Task<int> CountPendingOrderAsync(Guid userId, string sku);
        Task<OrderDTO> GetOrderAsync(string orderNo);
        Task<PagedList<OrderDTO>> GetOrdersAsync(GetOrdersQuery query);
    }
}
