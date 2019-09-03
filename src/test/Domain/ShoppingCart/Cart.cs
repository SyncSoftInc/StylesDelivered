using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Domain.ShoppingCart;
using System;
using System.Threading.Tasks;

namespace ShoppingCart
{
    public class Cart
    {
        private static readonly Guid _userId = Guid.Empty;

        private static readonly Lazy<IShoppingCartService> _lazyShoppingCartService = ObjectContainer.LazyResolve<IShoppingCartService>();
        private IShoppingCartService ShoppingCartService => _lazyShoppingCartService.Value;

        [Test]
        public async Task GetOrInit()
        {
            var dto = await ShoppingCartService.GetOrInitAsync(_userId).ConfigureAwait(false);

            Assert.IsNotNull(dto);
        }
    }
}