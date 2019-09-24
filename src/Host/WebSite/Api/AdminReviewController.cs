using Microsoft.AspNetCore.Mvc;
using SyncSoft.App.Components;
using SyncSoft.ECP.AspNetCore.Mvc.Controllers;
using SyncSoft.StylesDelivered.Command.Review;
using SyncSoft.StylesDelivered.DataAccess.Review;
using SyncSoft.StylesDelivered.DTO.Review;
using SyncSoft.StylesDelivered.Query.Review;
using SyncSoft.StylesDelivered.WebSite.Models;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite.Api
{
    [Area("Api")]
    public class AdminReviewController : ApiController
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IReviewDF> _lazyReviewDF = ObjectContainer.LazyResolve<IReviewDF>();
        private IReviewDF ReviewDF => _lazyReviewDF.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CRUD  -

        ///// <summary>
        ///// 创建Review
        ///// </summary>
        //[HttpPost("api/admin/review")]
        //public Task<string> CreateReviewAsync(CreateReviewCommand cmd) => base.RequestAsync(cmd);

        ///// <summary>
        ///// Upadte Review
        ///// </summary>
        //[HttpPut("api/admin/review")]
        //public Task<string> UpdateReviewAsync(UpdateReviewCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// Approve Review Status
        /// </summary>
        [HttpPatch("api/admin/review")]
        public Task<string> ApproveReviewAsync(ApproveReviewCommand cmd) => base.RequestAsync(cmd);

        /// <summary>
        /// 删除Review
        /// </summary>
        [HttpDelete("api/admin/review/{id}")]
        public Task<string> DeleteReviewAsync(DeleteReviewCommand cmd) => base.RequestAsync(cmd);

        #endregion
        // *******************************************************************************************************************************
        #region -  GetReview  -

        /// <summary>
        /// 获取Review
        /// </summary>
        [HttpGet("api/admin/review/{id}")]
        public Task<ReviewDTO> GetReviewAsync(Guid id) => ReviewDF.GetReviewAsync(id);

        /// <summary>
        /// 获取分页Review数据
        /// </summary>
        [HttpGet("api/admin/reviews")]
        public async Task<DataTables<ReviewDTO>> GetReviewsAsync(DataTableModel model)
        {
            var query = new GetReviewsQuery
            {
                PageSize = model.PageSize,
                PageIndex = model.PageIndex,
                OrderBy = model.OrderBy,
                Draw = model.Draw,
                Keyword = model.Keyword,
                SortDirection = model.SortDirection,
            };
            query.SetContext(User.Identity);

            var plist = await ReviewDF.GetReviewsAsync(query).ConfigureAwait(false);
            return new DataTables<ReviewDTO>(query.Draw, plist);
        }

        #endregion
    }
}
