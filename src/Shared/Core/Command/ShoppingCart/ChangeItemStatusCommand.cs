using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;

namespace SyncSoft.StylesDelivered.Command.Product
{
    public class ChangeItemStatusCommand : RequestCommand
    {
        public ShoppingCartItemDTO Item { get; set; }
    }
}
