using SyncSoft.StylesDelivered.Enum.Review;
using System;

namespace SyncSoft.StylesDelivered.DTO.Review
{
    public class ReviewDTO
    {
        public Guid ID { get; set; }
        public string OrderNo { get; set; }
        public Guid User_ID { get; set; }
        public string User { get; set; }
        public string SKU { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ReviewStatusEnum Status { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
