using SyncSoft.ECP.Queries;
using SyncSoft.StylesDelivered.Enum.Product;

namespace SyncSoft.StylesDelivered.Query.Product
{
    public class GetProductsQuery : PagedQuery
    {
        public int Draw { get; set; }

        public string Keyword { get; set; }

        public ProductStatusEnum? Status { get; set; }
    }
}
