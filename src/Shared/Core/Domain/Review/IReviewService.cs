using SyncSoft.StylesDelivered.Command.Review;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Review
{
    public interface IReviewService
    {
        Task<string> CreateReviewAsync(CreateReviewCommand cmd);
        Task<string> ApproveReviewAsync(ApproveReviewCommand cmd);
        Task<string> DeleteReviewAsync(DeleteReviewCommand cmd);
    }
}
