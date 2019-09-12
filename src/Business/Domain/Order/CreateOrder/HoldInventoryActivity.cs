using Logistics;
using SyncSoft.App;
using SyncSoft.App.Components;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Order;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order.CreateOrder
{
    public class HoldInventoryActivity : TccActivity
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        #endregion

        protected override async Task RunAsync(CancellationToken? cancellationToken)
        {
            var cmd = base.Context.Get<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);
            var list = new List<InventoryDTO>(cmd.Order.Items.Count);

            foreach (var item in cmd.Order.Items)
            {
                var dto = new InventoryDTO
                {
                    ItemNo = item.SKU,
                    Qty = item.Qty
                };
                var mr = await InventoryServiceClient.HoldAsync(dto);

                if (mr.MsgCode.IsSuccess())
                {
                    list.Add(dto);
                    Context.Set("HoldItems", list);
                }
                else
                {
                    throw new Exception("Hold inventory failed: " + mr.MsgCode);
                }
            }
        }

        protected override async Task RollbackAsync()
        {
            var cmd = base.Context.Get<CreateOrderCommand>(CONSTANTS.TRANSACTIONS.EntryCommand);
            var items = Context.Get<List<InventoryDTO>>("HoldItems");
            foreach (var item in items)
            {
                await InventoryServiceClient.HoldAsync(item);
            }
        }
    }
}
