using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class RemoveItemCommand : RequestCommand
    {
        public ShoppingCartItemDTO Item { get; set; }
    }
}
