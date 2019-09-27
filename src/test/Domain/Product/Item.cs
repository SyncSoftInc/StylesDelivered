using AutoFixture;
using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using System;
using System.Threading.Tasks;

namespace Product
{
    public class Item
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductItemService> _lazyProductItemService = ObjectContainer.LazyResolve<IProductItemService>();
        private IProductItemService ProductItemService => _lazyProductItemService.Value;

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        Fixture _fixture = new Fixture();
        private ProductItemDTO _productItem;

        #endregion
        // *******************************************************************************************************************************
        #region -  Setup & TearDown  -

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            // ππ‘ÏProductItem
            _productItem = _fixture.Create<ProductItemDTO>();
            _productItem.ASIN = "test_product";
            _productItem.SKU = "test_item_0001";
            _productItem.Alias = "test_alias";
            _productItem.Color = "test_color";
            _productItem.Size = "test_szie";
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  InsertItems  -

        [Test]
        public async Task InsertItems()
        {
            var asin = "new_item_1";

            for (int i = 0; i < 10; i++)
            {
                var msgCode = await ProductItemDAL.InsertItemAsync(new SyncSoft.StylesDelivered.DTO.Product.ProductItemDTO
                {
                    ASIN = asin,
                    SKU = $"item1_{i:D4}",
                    Color = $"color_{i}",
                    Size = $"size_{i}"
                }).ConfigureAwait(false);
                Assert.IsTrue(msgCode.IsSuccess());
            }
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  LifeCycle  -

        [Test]
        public async Task LifeCycle()
        {
            var msgCode = await ProductItemService.CreateItemAsync(_productItem).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            _productItem.Alias = _productItem.Alias + "_UPDATE";
            msgCode = await ProductItemService.UpdateItemAsync(_productItem).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            msgCode = await ProductItemService.DeleteItemAsync(_productItem.ASIN, _productItem.SKU).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Sync  -

        [Test]
        public async Task Sync()
        {
            var msgCode = await ProductItemService.SyncInventoriesAsync().ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess());

            msgCode = await ProductItemService.SyncHoldInventoriesAsync().ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess());
        }

        #endregion
    }
}