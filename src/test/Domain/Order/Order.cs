using AutoFixture;
using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.ECP;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Domain.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using SyncSoft.StylesDelivered.DTO.Product;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Order
{
    public class Order
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderService> _lazyOrderService = ObjectContainer.LazyResolve<IOrderService>();
        private IOrderService OrderService => _lazyOrderService.Value;

        private static readonly Lazy<IProductDAL> _lazyProductDAL = ObjectContainer.LazyResolve<IProductDAL>();
        private IProductDAL ProductDAL => _lazyProductDAL.Value;

        private static readonly Lazy<IProductItemDAL> _lazyProductItemDAL = ObjectContainer.LazyResolve<IProductItemDAL>();
        private IProductItemDAL ProductItemDAL => _lazyProductItemDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Field(s)  -

        Fixture _fixture = new Fixture();

        private ClaimsIdentity _identity = new ClaimsIdentity(new Claim[] {
            new Claim(CONSTANTs.Claims.UserID, "10000001-1001-1001-1001-100000000001"),
            new Claim(ClaimTypes.Name, "sa@admin.com"),
            new Claim(ClaimTypes.Role, "4")
        });

        private ProductDTO _product;
        private OrderDTO _order;

        #endregion
        // *******************************************************************************************************************************
        #region -  Setup & TearDown  -

        [OneTimeSetUp]
        public async Task SetupAsync()
        {
            _product = await ProductDAL.GetProductAsync("B07WJ6W9R7").ConfigureAwait(false);
            _product.Items = await ProductItemDAL.GetItemsAsync(_product.ASIN).ConfigureAwait(false);

            // 构造订单
            _order = _fixture.Create<OrderDTO>();
            _order.Shipping_Address1 = "45875 Northport Loop E";
            _order.Shipping_City = "Fremont";
            _order.Shipping_State = "CA";
            _order.Shipping_ZipCode = "94538";
            _order.Shipping_Country = "US";
            _order.Items = _product.Items.Select(x => new OrderItemDTO
            {
                OrderNo = _order.OrderNo,
                Qty = 1,
                ASIN = x.ASIN,
                Alias = x.Alias,
                Color = x.Color,
                ImageUrl = x.ImageUrl,
                Size = x.Size,
                SKU = x.SKU,
                Url = x.Url,
            }).ToList();
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateOrder  -

        [Test]
        public async Task LifeCycle()
        {
            var createCmd = new CreateOrderCommand
            {
                Order = _order
            };
            createCmd.SetContext(_identity);
            var mr = await OrderService.CreateOrderAsync(createCmd).ConfigureAwait(false);
            Assert.IsTrue(mr.IsSuccess, mr.MsgCode);
            _order.OrderNo = mr.Result;

            var approveCmd = new ApproveOrderCommand { OrderNo = _order.OrderNo };
            approveCmd.SetContext(_identity);
            var msgCode = await OrderService.ApproveOrderAsync(approveCmd).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);

            var deleteCmd = new DeleteOrderCommand { OrderNo = _order.OrderNo };
            deleteCmd.SetContext(_identity);
            msgCode = await OrderService.DeleteOrderAsync(deleteCmd).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        #endregion
    }
}
