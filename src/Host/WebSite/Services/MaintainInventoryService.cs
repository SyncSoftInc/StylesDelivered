using Inventory;
using Quartz;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.ECP.Quartz;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Services
{
    [DisallowConcurrentExecution]
    public class MaintainInventoryService : JobBase
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<MaintainInventoryService>();
        protected override ILogger Logger => _lazyLogger.Value;

        private static readonly Lazy<IProductItemService> _lazyProductItemService = ObjectContainer.LazyResolve<IProductItemService>();
        private IProductItemService ProductItemService => _lazyProductItemService.Value;

        private static readonly Lazy<InventoryService.InventoryServiceClient> _lazyInventoryServiceClient
            = ObjectContainer.LazyResolve<InventoryService.InventoryServiceClient>();
        private InventoryService.InventoryServiceClient InventoryServiceClient => _lazyInventoryServiceClient.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Execute  -

        protected override async Task<string> InnerExecuteAsync(IJobExecutionContext context)
        {
            // 清理仓库数量无效的库存记录
            var mr = await InventoryServiceClient.CleanWarehouseAsync(new InventoriesMSG { Warehouse = Constants.WarehouseID });
            var msgCode = mr.MsgCode;
            if (msgCode.IsSuccess())
            {
                // 所有库存同步
                var msgCodes = await Task.WhenAll(
                      ProductItemService.SyncInventoriesAsync()
                    , ProductItemService.SyncHoldInventoriesAsync()
                ).ConfigureAwait(false);

                msgCode = msgCodes.MsgCode();
            }
            return msgCode;
        }

        #endregion
    }
}
