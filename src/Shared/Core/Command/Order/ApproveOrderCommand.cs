using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Order
{
    public class ApproveOrderCommand : RequestCommand
    {
        public string OrderNo { get; set; }
    }
}
