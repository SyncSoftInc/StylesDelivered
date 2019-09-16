using Microsoft.AspNetCore.Mvc;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class OrderController : ApiController
    {
        /// <summary>
        /// Create Order
        /// </summary>
        [HttpPut("api/order")]
        public Task<string> CreateOrderAsync(CreateOrderCommand cmd)
        {
            cmd.Order.User_ID = User.Identity.UserID();

            foreach (var item in cmd.Order.Items)
            {
                item.Qty = 1;
            }

            return base.RequestAsync(cmd);
        }
    }
}
