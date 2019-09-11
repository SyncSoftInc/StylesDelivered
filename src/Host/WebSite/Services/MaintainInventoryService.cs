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

        #endregion

        protected override async Task<string> InnerExecuteAsync(IJobExecutionContext context)
        {
            var msgCode = await ProductItemService.SyncInventoriesAsync().ConfigureAwait(false);
            return msgCode;
        }
    }
}
