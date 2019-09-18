using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using System;

namespace SyncSoft.StylesDelivered.Domain.Order.ApproveOrder
{
    public class ApproveOrderTransaction : Transaction
    {
        //public const string Error = nameof(Error);

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<ApproveOrderTransaction>();
        public override ILogger Logger => _lazyLogger.Value;

        public ApproveOrderTransaction(ApproveOrderCommand cmd) : base(cmd.CorrelationId
            , new HoldInventoryActivity()
            , new ChangeOrderStatusActivity()
        //, new ShipConfirmActivity()
        )
        {
            SetState(CONSTANTS.TRANSACTIONS.EntryCommand, cmd);
        }
    }
}
