using SyncSoft.App.Redis;

namespace SyncSoft.StylesDelivered.Redis.Product.Inventory
{
    public class InventoryDB : RedisDB, IInventoryDB
    {
        public InventoryDB(string connStrName) : base(connStrName)
        { }
    }
}
