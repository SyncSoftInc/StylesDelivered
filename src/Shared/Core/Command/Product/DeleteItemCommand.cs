using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class DeleteItemCommand : RequestCommand
    {
        public string ASIN { get; set; }
        public string SKU { get; set; }
    }
}
