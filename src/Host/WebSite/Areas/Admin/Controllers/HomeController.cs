using Microsoft.AspNetCore.Mvc;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;

namespace SyncSoft.StylesDelivered.WebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : WebController
    {
        // *******************************************************************************************************************************
        #region -  Index  -

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
