using SyncSoft.App.Messaging;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class DeleteProductCommand : RequestCommand
    {
        public string ASIN { get; set; }
    }
}
