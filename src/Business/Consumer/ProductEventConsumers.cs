using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Domain.Product;
using SyncSoft.StylesDelivered.Event.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class ProductEventConsumers : IConsumer<ProductItemChangedEvent>
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  ProductItemChangedEvent  -


        public async Task<object> HandleAsync(IContext<ProductItemChangedEvent> context)
        {
            var msg = context.Message;

            var msgCode = await ProductService.RefreshItemsJsonAsync(msg.ASIN);

            return Task.FromResult<object>(msgCode);
        }

        #endregion
    }
}
