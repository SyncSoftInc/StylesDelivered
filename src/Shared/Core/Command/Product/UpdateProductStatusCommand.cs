using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class UpdateProductStatusCommand : RequestCommand
    {
        public string asin { get; set; }
        public int Status { get; set; }
    }
}
