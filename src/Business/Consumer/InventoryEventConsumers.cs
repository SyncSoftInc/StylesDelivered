using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Domain.Inventory;
using SyncSoft.StylesDelivered.Event.Inventory;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class InventoryEventConsumers : IConsumer<ItemInventoryChangedEvent>
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IItemInventoryFactory> _lazyItemInventoryFactory = ObjectContainer.LazyResolve<IItemInventoryFactory>();
        private IItemInventoryFactory ItemInventoryFactory => _lazyItemInventoryFactory.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  ItemInventoryChangedEvent  -

        public Task<object> HandleAsync(IContext<ItemInventoryChangedEvent> context)
        {
            var msg = context.Message;
            var inventory = ItemInventoryFactory.Create(msg.SKU);
            var msgCode = inventory.SetOnHandAsync(msg.InvQty);

            return Task.FromResult<object>(msgCode);
        }

        #endregion
    }
}
