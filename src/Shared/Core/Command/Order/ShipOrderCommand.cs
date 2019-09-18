using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Order
{
    public class ShipOrderCommand : RequestCommand
    {
        public string OrderNo { get; set; }
    }
}
