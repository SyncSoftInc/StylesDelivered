using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.Domain.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class OrderCommandConsumers : IConsumer<CreateOrderCommand>
    {
        private static readonly Lazy<IOrderService> _lazyOrderService = ObjectContainer.LazyResolve<IOrderService>();
        private IOrderService OrderService => _lazyOrderService.Value;

        public async Task<object> HandleAsync(IContext<CreateOrderCommand> context)
        {
            return await OrderService.CreateOrderAsync(context.Message).ConfigureAwait(false);
        }
    }
}
