using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.Review;
using SyncSoft.StylesDelivered.DataAccess.Review;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class ReviewController : ApiController
    {
        private static readonly Lazy<IReviewDF> _lazyReviewDF = ObjectContainer.LazyResolve<IReviewDF>();
        private IReviewDF ReviewDF => _lazyReviewDF.Value;

        /// <summary>
        /// 创建Review
        /// </summary>
        [HttpPost("api/review")]
        public Task<string> CreateReviewAsync(CreateReviewCommand cmd) => base.RequestAsync(cmd);
    }
}
