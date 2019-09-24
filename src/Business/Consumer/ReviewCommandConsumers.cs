using SyncSoft.App.Components;
using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.Command.Review;
using SyncSoft.StylesDelivered.Domain.Review;
using System;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Consumer
{
    public class ReviewCommandConsumers : IConsumer<DeleteReviewCommand>
         , IConsumer<UpdateReviewCommand>
         , IConsumer<ApproveReviewCommand>
    {
        private static readonly Lazy<IReviewService> _lazyReviewService = ObjectContainer.LazyResolve<IReviewService>();
        private IReviewService ReviewService => _lazyReviewService.Value;

        public async Task<object> HandleAsync(IContext<DeleteReviewCommand> context)
        {
            return await ReviewService.DeleteReviewAsync(context.Message).ConfigureAwait(false);
        }

        public Task<object> HandleAsync(IContext<UpdateReviewCommand> context)
        {
            throw new NotImplementedException();
        }

        public async Task<object> HandleAsync(IContext<ApproveReviewCommand> context)
        {
            return await ReviewService.ApproveReviewAsync(context.Message).ConfigureAwait(false);
        }
    }
}
