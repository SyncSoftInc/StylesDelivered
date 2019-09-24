using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Domain.Order;
using System;

namespace SyncSoft.StylesDelivered.WebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewController : WebController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IOrderExporter> _lazyOrderExporter = ObjectContainer.LazyResolve<IOrderExporter>();
        private IOrderExporter OrderExporter => _lazyOrderExporter.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Index  -

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
