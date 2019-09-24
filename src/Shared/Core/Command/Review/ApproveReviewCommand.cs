using SyncSoft.App.Messaging;
using System;

namespace SyncSoft.StylesDelivered.Command.Review
{
    public class ApproveReviewCommand : RequestCommand
    {
        public Guid ID { get; set; }
    }
}
