using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SyncSoft.StylesDelivered.WebSite.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Content("Access Denied");
            }
            else
            {
                return new EmptyResult();
            }
        }

        public IActionResult Address()
        {
            return View();
        }
    }
}
