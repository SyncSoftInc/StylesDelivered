using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Domain.Mail;
using SyncSoft.StylesDelivered.Event.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class OrderEventEmailConsumers : IConsumer<OrderApprovedEvent>
        , IConsumer<OrderShippedEvent>
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IEmailService> _lazyEmailService = ObjectContainer.LazyResolve<IEmailService>();
        private IEmailService EmailService => _lazyEmailService.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  OrderApprovedEvent  -

        public async Task<object> HandleAsync(IContext<OrderApprovedEvent> context)
        {
            var msg = context.Message;

            var msgCode = await EmailService.OrderSendAsync("Approved", msg.OrderItems).ConfigureAwait(false);

            return Task.FromResult<object>(msgCode);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  OrderShippedEvent  -

        public async Task<object> HandleAsync(IContext<OrderShippedEvent> context)
        {
            var msg = context.Message;

            var msgCode = await EmailService.OrderSendAsync("Shipped", msg.OrderItems).ConfigureAwait(false);

            return Task.FromResult<object>(msgCode);
        }

        #endregion
    }
}
