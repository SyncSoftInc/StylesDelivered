using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class DeleteProductItemCommand : RequestCommand
    {
        public string ItemNo { get; set; }
    }
}
