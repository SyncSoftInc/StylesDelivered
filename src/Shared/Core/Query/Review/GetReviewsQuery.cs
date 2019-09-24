using SyncSoft.ECP.Queries;

namespace SyncSoft.StylesDelivered.Query.Review
{
    public class GetReviewsQuery : PagedQuery
    {
        public int Draw { get; set; }

        public string Keyword { get; set; }
    }
}
