using SyncSoft.StylesDelivered.Enum.ShoppingCart;
using System;

namespace SyncSoft.StylesDelivered.DTO.ShoppingCart
{
    public class ShoppingCartItemDTO
    {
        public Guid Cart_ID { get; set; }
        public string ItemNo { get; set; }
        public ShoppingCartItemStatusEnum Status { get; set; }
        public int Qty { get; set; }
        public DateTime AddedOnUtc { get; set; }
    }
}
