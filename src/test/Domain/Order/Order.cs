using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.ECP;
using SyncSoft.StylesDelivered.Command.Order;
using SyncSoft.StylesDelivered.Domain.Order;
using SyncSoft.StylesDelivered.DTO.Order;
using System;
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

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateOrder  -

        [Test]
        public async Task CreateOrder()
        {
            var identity = new ClaimsIdentity(new Claim[] {
                new Claim(CONSTANTs.Claims.UserID, "d7637c80-7e64-435c-9474-cf82510256c1"),
                new Claim(ClaimTypes.Name, "lukiya.chen@syncsoftinc.com"),
                new Claim(ClaimTypes.Role, "4")
            });

            var cmd = new CreateOrderCommand
            {
                Order = new OrderDTO
                {
                    Shipping_Address1 = "45875 Northport Loop E",
                    Shipping_City = "Fremont",
                    Shipping_State = "CA",
                    Shipping_ZipCode = "94538",
                    Shipping_Country = "US",
                    Items = new OrderItemDTO[] {
                        new OrderItemDTO{ ASIN = "B07TQZMB43", SKU = "AMFBA10-0004", Qty = 3 },
                        new OrderItemDTO{ ASIN = "B07TQZMB43", SKU = "AMFBA10-0005", Qty = 2 },
                        new OrderItemDTO{ ASIN = "B07TQZMB43", SKU = "AMFBA10-0006", Qty = 4 },
                        new OrderItemDTO{ ASIN = "B07WJ6W9R7", SKU = "DC51-0096", Qty = 1 },
                        new OrderItemDTO{ ASIN = "B07WJ6W9R7", SKU = "DC51-0102", Qty = 8 },
                    }
                }
            };
            cmd.SetContext(identity);

            var msgCode = await OrderService.CreateOrderAsync(cmd);

            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  ApproveOrder  -

        [Test]
        public async Task ApproveOrder()
        {
            var orderNo = "6a12c235f01c4cafb5ac102ad2873609";

            var cmd = new ApproveOrderCommand { OrderNo = orderNo };
            var msgCode = await OrderService.ApproveOrderAsync(cmd);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  DeleteOrder  -

        [Test]
        public async Task DeleteOrder()
        {
            var orderNo = "6a12c235f01c4cafb5ac102ad2873609";

            var cmd = new DeleteOrderCommand { OrderNo = orderNo };
            var msgCode = await OrderService.DeleteOrderAsync(cmd);
            Assert.IsTrue(msgCode.IsSuccess(), msgCode);
        }

        #endregion
    }
}
