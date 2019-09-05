using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Inventory
{
    public interface IInventoryService
    {
        Task<string> CleanInventoriesAsync();
    }
}
