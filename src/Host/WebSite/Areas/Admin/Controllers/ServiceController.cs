using Microsoft.AspNetCore.Mvc;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;

namespace SyncSoft.StylesDelivered.WebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : WebController
    {
        // *******************************************************************************************************************************
        #region -  Items  -

        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
