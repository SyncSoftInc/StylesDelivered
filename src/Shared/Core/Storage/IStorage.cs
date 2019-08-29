using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Storage
{
    public interface IStorage
    {
        Task<string> SaveAsync(string key, byte[] data);
        Task<string> DeleteAsync(string key);
    }
}
