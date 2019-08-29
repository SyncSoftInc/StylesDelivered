using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.Redis.Product.Inventory;
using System;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStylesDeliveredRedis(this CommonConfigurator configurator, Action<DBOptions> configOptions = null, DBOptions options = null)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStylesDeliveredRedis));

            options = options ?? new DBOptions();
            configOptions?.Invoke(options);

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IInventoryDB>(() => new InventoryDB(options.DBConnStrName), LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IInventoryQueryDAL, InventoryQueryDAL>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
