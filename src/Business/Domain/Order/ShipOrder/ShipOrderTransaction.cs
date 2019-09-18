using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using System;

namespace SyncSoft.StylesDelivered.Domain.Order.ShipOrder
{
    public class ShipOrderTransaction : Transaction
    {
        //public const string Error = nameof(Error);

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<ShipOrderTransaction>();
        public override ILogger Logger => _lazyLogger.Value;

        public ShipOrderTransaction(ShipOrderCommand cmd) : base(cmd.CorrelationId
            , new ChangeOrderStatusActivity()
            , new ShipConfirmActivity()
        )
        {
            Context.Set(CONSTANTS.TRANSACTIONS.EntryCommand, cmd);
        }
    }
}
