using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace Product
{
    public class Item
    {
        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        [Test]
        public async Task SyncInventories()
        {
            var msgCode = await ProductService.SyncInventoriesAsync().ConfigureAwait(false);

            Assert.IsTrue(msgCode.IsSuccess());
        }
    }
}