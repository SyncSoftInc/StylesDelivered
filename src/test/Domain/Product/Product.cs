using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace Product
{
    public class Product
    {
        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        [Test]
        public async Task RefreshItemsJson()
        {
            var msgCode = await ProductService.RefreshProductAsync("test_aaa").ConfigureAwait(false);

            Assert.IsTrue(msgCode.IsSuccess());
        }

        [Test]
        public async Task LifeCycle()
        {
            throw new NotImplementedException();
            //var msgCode = await ProductItemService.CreateItemAsync(_productItem).ConfigureAwait(false);
            //Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            //_productItem.Alias = _productItem.Alias + "_UPDATE";
            //msgCode = await ProductItemService.UpdateItemAsync(_productItem).ConfigureAwait(false);
            //Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            //msgCode = await ProductItemService.DeleteItemAsync(_productItem.ASIN, _productItem.SKU).ConfigureAwait(false);
            //Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }
    }
}