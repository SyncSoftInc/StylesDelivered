using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using System;
using System.Collections.Generic;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class CreateOrderTransaction : TccTransaction
    {
        public const string Error = nameof(Error);

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<CreateOrderTransaction>();
        public override ILogger Logger => _lazyLogger.Value;

        public CreateOrderTransaction(CreateOrderCommand cmd) : base(cmd.CorrelationId)
        {
            base.Context.Set(CONSTANTS.TRANSACTIONS.EntryCommand, cmd);
        }

        protected override IEnumerable<TransactionActivity> BuildActivities()
        {
            yield return new SaveOrderActivity();
            yield return new HoldInventoryActivity();
        }
    }
}
