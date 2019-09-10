using SyncSoft.StylesDelivered.Enum.Product;

namespace SyncSoft.StylesDelivered.WebSite.Models
{
    public class ProductQueryModel : DataTableModel
    {
        public ProductStatusEnum? Status { get; internal set; }
    }
}
