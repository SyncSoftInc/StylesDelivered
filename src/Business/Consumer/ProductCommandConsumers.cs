using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Command.Product;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class ProductCommandConsumers : IConsumer<DeleteProductCommand>
         , IConsumer<CreateProductCommand>
         , IConsumer<UpdateProductCommand>
         , IConsumer<UploadProductImageCommand>
    {
        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        public async Task<object> HandleAsync(IContext<DeleteProductCommand> context)
        {
            return await ProductService.DeleteProductAsync(context.Message.ASIN).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<CreateProductCommand> context)
        {
            return await ProductService.CreateProductAsync(context.Message.Product).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<UpdateProductCommand> context)
        {
            return await ProductService.UpdateProductAsync(context.Message.Product).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<UploadProductImageCommand> context)
        {
            return await ProductService.UploadImageAsync(context.Message).ConfigureAwait(false);
        }
    }
}
