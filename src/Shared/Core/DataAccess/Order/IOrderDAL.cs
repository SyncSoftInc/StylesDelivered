using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.Enum.Order;
using SyncSoft.StylesDelivered.Query.Order;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Order
{
    public interface IOrderDAL
    {
        Task<string> InsertAsync(OrderDTO dto);
        Task<string> UpdateOrderStatusAsync(string orderNo, OrderStatusEnum status);
        Task<string> DeleteOrderAsync(string orderNo);

        Task<int> CountOrderedItemsAsync(Guid userId, string sku);
        Task<OrderDTO> GetOrderAsync(string orderNo);
        Task<IList<OrderDTO>> GetOrdersAsync(OrderStatusEnum status);
        Task<PagedList<OrderDTO>> GetOrdersAsync(GetOrdersQuery query);
    }
}
