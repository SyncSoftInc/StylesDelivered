using SyncSoft.StylesDelivered.DTO.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Order
{
    public interface IOrderDAL
    {
        Task InsertAsync(OrderDTO dto);
        Task DeleteOrderAsync(string orderNo);
        Task<OrderDTO> GetPendingOrderAsync(Guid userId);
    }
}
