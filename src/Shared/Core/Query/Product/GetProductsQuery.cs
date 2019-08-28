using SyncSoft.ECP.Queries;

namespace SyncSoft.StylesDelivered.Query.Product
{
    public class GetProductsQuery : PagedQuery
    {
        public int Draw { get; set; }

        public string Keyword { get; set; }
    }
}
