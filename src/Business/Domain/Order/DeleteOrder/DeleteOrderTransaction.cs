using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using System;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.Domain.Order.DeleteOrder
{
    public class DeleteOrderTransaction : TccTransaction
    {
        public const string Error = nameof(Error);

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<DeleteOrderTransaction>();
        public override ILogger Logger => _lazyLogger.Value;

        public DeleteOrderTransaction(DeleteOrderCommand cmd) : base(cmd.CorrelationId)
        {
            base.Context.Set(CONSTANTS.TRANSACTIONS.EntryCommand, cmd);
        }

        protected override IEnumerable<TransactionActivity> BuildActivities()
        {
            yield return new DeleteOrderActivity();
            yield return new UnHoldInventoryActivity();
        }
    }
}
