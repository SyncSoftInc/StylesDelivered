using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class ChangeItemQtyCommand : RequestCommand
    {
        public ShoppingCartItemDTO Item { get; set; }
    }
}
