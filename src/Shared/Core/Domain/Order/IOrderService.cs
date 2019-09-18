using SyncSoft.App;
using SyncSoft.StylesDelivered.Command.Order;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order
{
    public interface IOrderService
    {
        Task<MsgResult<string>> CreateOrderAsync(CreateOrderCommand cmd);
        Task<string> ApproveOrderAsync(ApproveOrderCommand cmd);
        Task<string> ShipOrderAsync(ShipOrderCommand cmd);
        Task<string> DeleteOrderAsync(DeleteOrderCommand cmd);
    }
}
