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
        Task InsertAsync(OrderDTO dto);
        Task UpdateOrderStatusAsync(string orderNo, OrderStatusEnum status);
        Task DeleteOrderAsync(string orderNo);

        Task<OrderDTO> GetPendingOrderAsync(Guid userId);
        Task<OrderDTO> GetOrderAsync(string orderNo);
        Task<PagedList<OrderDTO>> GetOrdersAsync(GetOrdersQuery query);
    }
}
