using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Order
{
    public interface IOrderExporter
    {
        Task<byte[]> ExportAsync();
    }
}
