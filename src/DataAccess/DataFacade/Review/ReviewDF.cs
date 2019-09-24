using SyncSoft.App.Components;
using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DataAccess.Review;
using SyncSoft.StylesDelivered.DTO.Review;
using SyncSoft.StylesDelivered.Query.Review;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataFacade.Review
{
    public class ReviewDF : IReviewDF
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IReviewDAL> _lazyReviewDAL = ObjectContainer.LazyResolve<IReviewDAL>();
        private IReviewDAL ReviewDAL => _lazyReviewDAL.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  Get  -

        public async Task<ReviewDTO> GetReviewAsync(Guid id)
        {
            var dto = await ReviewDAL.GetReviewAsync(id).ConfigureAwait(false);
            return dto;
        }

        public async Task<PagedList<ReviewDTO>> GetReviewsAsync(GetReviewsQuery query)
        {
            var rs = await ReviewDAL.GetReviewsAsync(query).ConfigureAwait(false);
            return rs;
        }

        #endregion
    }
}
