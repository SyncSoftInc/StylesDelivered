using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DataAccess.Order;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Event.Order;
using System;
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

        #endregion
        // *******************************************************************************************************************************
        #region -  OrderChangedEvent  -

        public async Task<object> HandleAsync(IContext<OrderApprovedEvent> context)
        {
            var msg = context.Message;

            var orderItems = msg.OrderItems;

            //ProductItemDAL.SetItemHoldInvQtysdAsync(orderItems.ToDictionary<string, long>(x => x.SKU, x => x.));

            //var msgCode = await ProductItemService.SyncHoldInventoriesAsync();

            return Task.FromResult<object>("");
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  OrderShippedEvent  -

        public async Task<object> HandleAsync(IContext<OrderShippedEvent> context)
        {
            var msg = context.Message;

            //var msgCode = await ProductItemService.SyncHoldInventoriesAsync();
            //if (msgCode.IsSuccess())
            //{
            //    msgCode = await ProductItemService.SyncInventoriesAsync();
            //}

            return Task.FromResult<object>("");
        }

        #endregion
    }
}
