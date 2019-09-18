using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using System;

namespace SyncSoft.StylesDelivered.Domain.Order.DeleteOrder
{
    public class DeleteOrderTransaction : Transaction
    {
        //public const string Error = nameof(Error);

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<DeleteOrderTransaction>();
        public override ILogger Logger => _lazyLogger.Value;

        public DeleteOrderTransaction(DeleteOrderCommand cmd) : base(cmd.CorrelationId
            , new DeleteOrderActivity()
        //, new UnHoldInventoryActivity()
        )
        {
            SetState(CONSTANTS.TRANSACTIONS.EntryCommand, cmd);
        }
    }
}
