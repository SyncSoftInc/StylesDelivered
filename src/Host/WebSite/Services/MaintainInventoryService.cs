using Quartz;
using SyncSoft.App.Components;
using SyncSoft.App.Logging;
using SyncSoft.ECP.Quartz;
using SyncSoft.StylesDelivered.Domain.Inventory;
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

        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        private static readonly Lazy<IInventoryService> _lazyInventoryService = ObjectContainer.LazyResolve<IInventoryService>();
        private IInventoryService InventoryService => _lazyInventoryService.Value;

        #endregion

        protected override async Task<string> InnerExecuteAsync(IJobExecutionContext context)
        {
            var msgCode = await InventoryService.CleanInventoriesAsync().ConfigureAwait(false);
            if (msgCode.IsSuccess())
            {
                msgCode = await ProductService.SyncInventoriesAsync().ConfigureAwait(false);
            }
            return msgCode;
        }
    }
}
