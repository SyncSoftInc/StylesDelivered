using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.Product;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class UpdateProductCommand : RequestCommand
    {
        public ProductDTO Product { get; set; }
    }
}
