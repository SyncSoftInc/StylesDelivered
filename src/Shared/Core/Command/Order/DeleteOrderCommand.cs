using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Order
{
    public class DeleteOrderCommand : RequestCommand
    {
        public string OrderNo { get; set; }
    }
}
