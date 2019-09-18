using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Domain.Order;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Areas.Product.Controllers
{
    [Area("Admin")]
    public class OrderController : WebController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderExporter> _lazyOrderExporter = ObjectContainer.LazyResolve<IOrderExporter>();
        private IOrderExporter OrderExporter => _lazyOrderExporter.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Items  -

        public IActionResult Index()
        {
            return View();
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Detail  -

        public IActionResult Detail(string id)
        {
            return View(model: id);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  Export  -

        [HttpGet]
        public async Task<IActionResult> ExportOrders()
        {
            var bytes = await OrderExporter.ExportAsync().ConfigureAwait(false);

            Response.Headers["Content-Disposition"] = "attachment";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(bytes, contentType, "ApprovedOrders.xlsx");
        }

        #endregion
    }
}
