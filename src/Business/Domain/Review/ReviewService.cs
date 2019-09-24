using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.App.Transactions;
using SyncSoft.StylesDelivered.Command.Review;
using SyncSoft.StylesDelivered.DataAccess.Review;
using SyncSoft.StylesDelivered.Enum.Review;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Review
{
    public class ReviewService : IReviewService
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IControllerFactory> _lazyControllerFactory = ObjectContainer.LazyResolve<IControllerFactory>();
        private IControllerFactory ControllerFactory => _lazyControllerFactory.Value;

        private static readonly Lazy<IReviewDAL> _lazyReviewDAL = ObjectContainer.LazyResolve<IReviewDAL>();
        private IReviewDAL ReviewDAL => _lazyReviewDAL.Value;

        private static readonly Lazy<IMessageDispatcher> _lazyMessageDispatcher = ObjectContainer.LazyResolve<IMessageDispatcher>();
        private IMessageDispatcher MessageDispatcher => _lazyMessageDispatcher.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateReview  -

        public async Task<string> CreateReviewAsync(CreateReviewCommand cmd)
        {
            var userId = cmd.Identity.UserID();

            var count = await ReviewDAL.GetOrderItemReviewAsync(cmd.Review.OrderNo, cmd.Review.SKU, userId).ConfigureAwait(false);
            if (count > 0) return "You have already reviewed this item.";
            // ^^^^^^^^^^

            cmd.Review.ID = Guid.NewGuid();
            cmd.Review.User_ID = userId;
            cmd.Review.User = cmd.Identity.UserNickName();
            cmd.Review.Status = ReviewStatusEnum.Pending;
            cmd.Review.CreatedOnUtc = DateTime.UtcNow;

            return await ReviewDAL.InsertReviewAsync(cmd.Review).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  ApproveReview  -

        public async Task<string> ApproveReviewAsync(ApproveReviewCommand cmd)
        {
            var dto = await ReviewDAL.GetReviewAsync(cmd.ID).ConfigureAwait(false);
            if (dto.IsNull()) return MsgCodes.ReviewNotExists;
            // ^^^^^^^^^^

            dto.Status = ReviewStatusEnum.Approved;
            return await ReviewDAL.UpdateReviewStatusAsync(dto).ConfigureAwait(false);
        }

        #endregion
        // *******************************************************************************************************************************
        #region -  DeleteReview  -

        public async Task<string> DeleteReviewAsync(DeleteReviewCommand cmd)
        {
            var dto = await ReviewDAL.GetReviewAsync(cmd.ID).ConfigureAwait(false);
            if (dto.IsNull()) return MsgCodes.ReviewNotExists;
            // ^^^^^^^^^^

            return await ReviewDAL.DeleteReviewAsync(cmd.ID).ConfigureAwait(false);
        }

        #endregion
    }
}
