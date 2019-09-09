using SyncSoft.ECP.Queries;

namespace SyncSoft.StylesDelivered.Query.Product
{
    public class GetProductItemsQuery : PagedQuery
    {
        public int Draw { get; set; }

        public string Keyword { get; set; }
        public string ASIN { get; set; }
    }
}
