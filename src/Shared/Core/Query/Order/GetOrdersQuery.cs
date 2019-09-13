using SyncSoft.ECP.Queries;

namespace SyncSoft.StylesDelivered.Query.Order
{
    public class GetOrdersQuery : PagedQuery
    {
        public int Draw { get; set; }

        public string Keyword { get; set; }
    }
}
