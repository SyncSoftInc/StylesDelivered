using Microsoft.AspNetCore.Mvc;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;

namespace SyncSoft.StylesDelivered.WebSite.Areas.Product.Controllers
{
    [Area("Admin")]
    public class ProfileController : WebController
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
