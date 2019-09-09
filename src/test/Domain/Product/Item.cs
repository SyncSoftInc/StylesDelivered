using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace Product
{
    public class Item
    {
        private static readonly Lazy<IProductItemService> _lazyProductItemService = ObjectContainer.LazyResolve<IProductItemService>();
        private IProductItemService ProductItemService => _lazyProductItemService.Value;

        [Test]
        public async Task SyncInventories()
        {
            var msgCode = await ProductItemService.SyncInventoriesAsync().ConfigureAwait(false);

            Assert.IsTrue(msgCode.IsSuccess());
        }
    }
}