using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.Order;

namespace SyncSoft.StylesDelivered.Command.Order
{
    public class CreateOrderCommand : RequestCommand
    {
        public OrderDTO Order { get; set; }
    }
}
