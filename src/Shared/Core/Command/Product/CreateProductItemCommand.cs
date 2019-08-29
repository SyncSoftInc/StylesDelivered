using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.Product;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class CreateProductItemCommand : RequestCommand
    {
        public ProductItemDTO ProductItem { get; set; }
    }
}
