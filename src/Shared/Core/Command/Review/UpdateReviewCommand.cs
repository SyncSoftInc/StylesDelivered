using SyncSoft.App.Messaging;
using SyncSoft.StylesDelivered.DTO.Review;

namespace SyncSoft.StylesDelivered.Command.Review
{
    public class UpdateReviewCommand : RequestCommand
    {
        public ReviewDTO Review { get; set; }
    }
}
