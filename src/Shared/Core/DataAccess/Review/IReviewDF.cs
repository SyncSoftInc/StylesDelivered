using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Review;
using SyncSoft.StylesDelivered.Query.Review;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Review
{
    public interface IReviewDF
    {
        Task<ReviewDTO> GetReviewAsync(Guid id);
        Task<PagedList<ReviewDTO>> GetReviewsAsync(GetReviewsQuery query);
    }
}
