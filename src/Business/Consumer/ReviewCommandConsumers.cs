using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Command.Review;
using SyncSoft.StylesDelivered.Domain.Review;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class ReviewCommandConsumers : IConsumer<CreateReviewCommand>
         , IConsumer<DeleteReviewCommand>
         , IConsumer<ApproveReviewCommand>
    {
        private static readonly Lazy<IReviewService> _lazyReviewService = ObjectContainer.LazyResolve<IReviewService>();
        private IReviewService ReviewService => _lazyReviewService.Value;

        public async Task<object> HandleAsync(IContext<CreateReviewCommand> context)
        {
            return await ReviewService.CreateReviewAsync(context.Message).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<DeleteReviewCommand> context)
        {
            return await ReviewService.DeleteReviewAsync(context.Message).ConfigureAwait(false);
        }

        public async Task<object> HandleAsync(IContext<ApproveReviewCommand> context)
        {
            return await ReviewService.ApproveReviewAsync(context.Message).ConfigureAwait(false);
        }
    }
}
