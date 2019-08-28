using Microsoft.AspNetCore.Mvc;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;

namespace SyncSoft.StylesDelivered.WebSite.Areas.Product.Controllers
{
    [Area("Admin")]
    public class ProductController : WebController
    {
        // *******************************************************************************************************************************
        #region -  Items  -

        public IActionResult Items()
        {
            return View();
        }

        #endregion
    }
}
