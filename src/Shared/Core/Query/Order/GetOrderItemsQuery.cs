using SyncSoft.ECP.Queries;

namespace SyncSoft.StylesDelivered.Query.Order
{
    public class GetOrderItemsQuery : PagedQuery
    {
        public int Draw { get; set; }

        public string Keyword { get; set; }

        public string OrderNo { get; set; }
    }
}
