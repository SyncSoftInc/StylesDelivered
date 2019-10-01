using AutoFixture;
using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Domain.Product;
using SyncSoft.StylesDelivered.DTO.Product;
using SyncSoft.StylesDelivered.Enum.Product;
using System;
using System.Threading.Tasks;

namespace Product
{
    public class Product
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IProductService> _lazyProductService = ObjectContainer.LazyResolve<IProductService>();
        private IProductService ProductService => _lazyProductService.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        Fixture _fixture = new Fixture();
        private ProductDTO _product;

        #endregion
        // *******************************************************************************************************************************
        #region -  Setup & TearDown  -

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            // 构造Product
            _product = _fixture.Create<ProductDTO>();
            _product.ASIN = "test_product_ 0001";
            _product.ProductName = "test_productName";
            _product.Description = "test_description";
            //_product.ImageUrl = "";
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  RefreshItemsJson  -

        [Test]
        public async Task RefreshItemsJson()
        {
            var msgCode = await ProductService.RefreshProductAsync(_product.ASIN).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess());
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  UpdateStatus  -

        [Test]
        public async Task UpdateStatus()
        {
            var msgCode = await ProductService.UpdateStatusAsync(new SyncSoft.StylesDelivered.Command.Product.UpdateProductStatusCommand
            {
                ASIN = _product.ASIN,
                Status = ProductStatusEnum.Active
            }).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess());
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  LifeCycle  -

        [Test]
        public async Task LifeCycle()
        {
            var msgCode = await ProductService.CreateProductAsync(_product).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            _product.Description = _product.Description + "_UPDATE";
            msgCode = await ProductService.UpdateProductAsync(_product).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            msgCode = await ProductService.DeleteProductAsync(_product.ASIN).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        #endregion
    }
}