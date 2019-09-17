﻿using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using System;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class CreateOrderTransaction : Transaction
    {
        //public const string Error = nameof(Error);

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<CreateOrderTransaction>();
        public override ILogger Logger => _lazyLogger.Value;

        public CreateOrderTransaction(CreateOrderCommand cmd) : base(cmd.CorrelationId
            , new CheckEligibilityActivity()
            , new SaveOrderActivity()
        //, new HoldInventoryActivity() // 暂时不锁定库存
        )
        {
            Context.Set(CONSTANTS.TRANSACTIONS.EntryCommand, cmd);
        }
    }
}
