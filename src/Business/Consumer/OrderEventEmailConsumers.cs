using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Event.Order;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class OrderEventEmailConsumers : IConsumer<OrderApprovedEvent>
        , IConsumer<OrderShippedEvent>
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -


        #endregion
        // *******************************************************************************************************************************
        #region -  OrderApprovedEvent  -

        public Task<object> HandleAsync(IContext<OrderApprovedEvent> context)
        {
            var msg = context.Message;



            return Task.FromResult<object>(MsgCodes.SUCCESS);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  OrderShippedEvent  -

        public Task<object> HandleAsync(IContext<OrderShippedEvent> context)
        {
            var msg = context.Message;



            return Task.FromResult<object>(MsgCodes.SUCCESS);
        }

        #endregion
    }
}
