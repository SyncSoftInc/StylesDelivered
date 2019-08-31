using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class ProductCommandConsumers : IConsumer<DeleteProductItemCommand>
         , IConsumer<CreateProductItemCommand>
         , IConsumer<UpdateProductItemCommand>
    {
        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        public async Task<object> HandleAsync(IContext<DeleteProductItemCommand> context)
        {
            return await ProductService.DeleteItemAsync(context.Message.ItemNo).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<CreateProductItemCommand> context)
        {
            return await ProductService.CreateItemAsync(context.Message.ProductItem).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<UpdateProductItemCommand> context)
        {
            return await ProductService.UpdateItemAsync(context.Message.ProductItem).ConfigureAwait(false);
        }
    }
}
