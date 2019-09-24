using Microsoft.AspNetCore.Mvc;

namespace SyncSoft.StylesDelivered.WebSite.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index(string orderNo, string sku)
        {
            return View(model: new { OrderNo = orderNo, SKU = sku });
        }
    }
}
