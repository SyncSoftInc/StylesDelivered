﻿using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.ShoppingCart;

namespace SyncSoft.StylesDelivered.Command.ShoppingCart
{
    public class ShopNowCommand : RequestCommand
    {
        public ShoppingCartItemDTO Item { get; set; }
    }
}
