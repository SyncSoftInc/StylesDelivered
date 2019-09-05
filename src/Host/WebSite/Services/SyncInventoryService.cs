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
    public class SyncInventoryService : JobBase
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<ILogger> _lazyLogger = ObjectContainer.LazyResolveLogger<SyncInventoryService>();
        protected override ILogger Logger => _lazyLogger.Value;

        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        #endregion

        protected override async Task<string> InnerExecuteAsync(IJobExecutionContext context)
        {
            return await ProductService.SyncInventoriesAsync().ConfigureAwait(false);
        }
    }
}
