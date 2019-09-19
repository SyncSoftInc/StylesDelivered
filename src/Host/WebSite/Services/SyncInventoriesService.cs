using Grpc.Core;
using Inventory;
using Quartz;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.ECP.Quartz;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Services
{
    [DisallowConcurrentExecution]
    public class SyncInventoriesService : JobBase
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<MaintainInventoryService>();
        protected override ILogger Logger => _lazyLogger.Value;

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        private static readonly Lazy<ISyncInvQueue> _lazySyncInvQueue = ObjectContainer.LazyResolve<ISyncInvQueue>();
        private ISyncInvQueue SyncInvQueue => _lazySyncInvQueue.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Execute  -

        protected override async Task<string> InnerExecuteAsync(IJobExecutionContext context)
        {
            var itemNos = SyncInvQueue.PopAll();    // 从待同步队列中取出ItemNos RL: {D2AEB42F-DB1C-41B4-8EE3-97DA9980C818}
            if (itemNos.IsPresent())
            {
                var invs = new InventoriesMSG();
                foreach (var itemNo in itemNos)
                {
                    invs.Inventories.Add(new InventoryMSG
                    {
                        ItemNo = itemNo,
                    });
                }

                // 开始同步
                var msgCodes = await Task.WhenAll(
                      SyncInventoriesAsync(invs, InventoryServiceClient.GetOnHandQtysAsync, ProductItemDAL.SetItemInvQtysdAsync)
                    , SyncInventoriesAsync(invs, InventoryServiceClient.GetOnHoldQtysAsync, ProductItemDAL.SetItemHoldInvQtysdAsync)
                ).ConfigureAwait(false);

                return msgCodes.MsgCode();
            }

            return MsgCodes.SUCCESS;
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  SyncInventories  -

        private async Task<string> SyncInventoriesAsync(InventoriesMSG invs
            , Func<InventoriesMSG, Metadata, DateTime?, CancellationToken, AsyncUnaryCall<InventoriesMSG>> remoteCall
            , Func<IDictionary<string, long>, Task<string>> localCall
)
        {
            invs = await remoteCall(invs, null, null, default);

            var msgCode = await localCall(invs.Inventories.ToDictionary(x => x.ItemNo, x => x.Qty));
            return msgCode;
        }

        #endregion
    }
}
