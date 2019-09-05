using SyncSoft.App.Redis;

namespace SyncSoft.StylesDelivered.Redis.Inventory
{
    public class InventoryDB : RedisDB, IInventoryDB
    {
        public InventoryDB(string connStrName) : base(connStrName)
        { }
    }
}
