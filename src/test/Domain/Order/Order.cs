using NUnit.Framework;
using SyncSoft.App.Components;
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
        [Test]
        public async Task CreateOrder()
        {
            var identity = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "d7637c80-7e64-435c-9474-cf82510256c1"),
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
                        new OrderItemDTO{ SKU = "AMFBA10-0004" },
                        new OrderItemDTO{ SKU = "AMFBA10-0005" },
                        new OrderItemDTO{ SKU = "AMFBA10-0006" },
                        new OrderItemDTO{ SKU = "DC51-0096" },
                        new OrderItemDTO{ SKU = "DC51-0102" },
                    }
                }
            };
            cmd.SetContext(identity);

            var msgCode = await OrderService.CreateOrderAsync(cmd);

            Assert.IsTrue(msgCode.IsSuccess());
        }
    }
}
