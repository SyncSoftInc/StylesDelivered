using Inventory;
using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Inventory;
using SyncSoft.StylesDelivered.Event.Order;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class OrderEventConsumers : IConsumer<OrderApprovedEvent>
        , IConsumer<OrderShippedEvent>
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        private static readonly Lazy<IOrderDAL> _lazyOrderDAL = ObjectContainer.LazyResolve<IOrderDAL>();
        private IOrderDAL OrderDAL => _lazyOrderDAL.Value;

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        private static readonly Lazy<ISyncInvQueue> _lazySyncInvQueue = ObjectContainer.LazyResolve<ISyncInvQueue>();
        private ISyncInvQueue SyncInvQueue => _lazySyncInvQueue.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  OrderApprovedEvent  -

        public Task<object> HandleAsync(IContext<OrderApprovedEvent> context)
        {
            var msg = context.Message;

            // 放入待同步队列 RL: {D2AEB42F-DB1C-41B4-8EE3-97DA9980C818}
            SyncInvQueue.Push(msg.OrderItems.Select(x => x.SKU).ToArray());

            return Task.FromResult<object>(MsgCodes.SUCCESS);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  OrderShippedEvent  -

        public Task<object> HandleAsync(IContext<OrderShippedEvent> context)
        {
            var msg = context.Message;

            // 放入待同步队列 RL: {D2AEB42F-DB1C-41B4-8EE3-97DA9980C818}
            SyncInvQueue.Push(msg.OrderItems.Select(x => x.SKU).ToArray());

            return Task.FromResult<object>(MsgCodes.SUCCESS);
        }

        #endregion
    }
}
