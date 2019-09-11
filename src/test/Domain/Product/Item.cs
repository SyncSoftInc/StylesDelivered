using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Product;
using System;
using System.Threading.Tasks;

namespace Product
{
    public class Item
    {
        private static readonly Lazy<IProductItemService> _lazyProductItemService = ObjectContainer.LazyResolve<IProductItemService>();
        private IProductItemService ProductItemService => _lazyProductItemService.Value;

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        [Test]
        public async Task SyncInventories()
        {
            var msgCode = await ProductItemService.SyncInventoriesAsync().ConfigureAwait(false);

            Assert.IsTrue(msgCode.IsSuccess());
        }

        [Test]
        public async Task InsertItems()
        {
            var asin = "new_item_1";

            for (int i = 0; i < 10; i++)
            {
                var msgCode = await ProductItemDAL.InsertItemAsync(new SyncSoft.StylesDelivered.DTO.Product.ProductItemDTO
                {
                    ASIN = asin,
                    SKU = $"item1_{i}",
                    Color = $"color_{i}",
                    Size = $"size_{i}"
                }).ConfigureAwait(false);
                Assert.IsTrue(msgCode.IsSuccess());
            }
        }
    }
}