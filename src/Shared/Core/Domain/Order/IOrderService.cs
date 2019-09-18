using SyncSoft.StylesDelivered.Command.Order;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(CreateOrderCommand cmd);
        Task<string> ApproveOrderAsync(ApproveOrderCommand cmd);
        Task<string> ShipOrderAsync(ShipOrderCommand cmd);
        Task<string> DeleteOrderAsync(DeleteOrderCommand cmd);
    }
}
