using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class ProductItemCommandConsumers : IConsumer<DeleteItemCommand>
         , IConsumer<CreateItemCommand>
         , IConsumer<UpdateItemCommand>
    {
        private static readonly Lazy<IProductItemService> _lazyProductItemService = ObjectContainer.LazyResolve<IProductItemService>();
        private IProductItemService ProductItemService => _lazyProductItemService.Value;

        public async Task<object> HandleAsync(IContext<DeleteItemCommand> context)
        {
            return await ProductItemService.DeleteItemAsync(context.Message.ASIN, context.Message.SKU).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<CreateItemCommand> context)
        {
            return await ProductItemService.CreateItemAsync(context.Message.ProductItem).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<UpdateItemCommand> context)
        {
            return await ProductItemService.UpdateItemAsync(context.Message.ProductItem).ConfigureAwait(false);
        }
    }
}
