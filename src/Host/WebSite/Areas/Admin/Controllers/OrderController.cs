﻿using Microsoft.AspNetCore.Mvc;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;

namespace SyncSoft.StylesDelivered.WebSite.Areas.Product.Controllers
{
    [Area("Admin")]
    public class OrderController : WebController
    {
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
    }
}