using SyncSoft.ECP.DTOs;
using SyncSoft.StylesDelivered.DTO.Review;
using SyncSoft.StylesDelivered.Query.Review;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.DataAccess.Review
{
    public interface IReviewDAL
    {
        Task<string> InsertReviewAsync(ReviewDTO dto);
        Task<string> UpdateReviewAsync(ReviewDTO dto);
        Task<string> UpdateReviewStatusAsync(ReviewDTO dto);
        Task<string> DeleteReviewAsync(Guid id);

        Task<ReviewDTO> GetReviewAsync(Guid id);
        Task<int> GetOrderItemReviewAsync(string orderNo, string sku, Guid userId);
        Task<PagedList<ReviewDTO>> GetReviewsAsync(GetReviewsQuery query);
    }
}
