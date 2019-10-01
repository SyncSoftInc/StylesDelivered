using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Enum.Product;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class UpdateProductStatusCommand : RequestCommand
    {
        public string ASIN { get; set; }
        public ProductStatusEnum Status { get; set; }
    }
}
