using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.MongoDB;
using SyncSoft.StylesDelivered.MongoDB.Product;
using SyncSoft.StylesDelivered.MongoDB.User;
using System;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStylesDeliveredMongoDB(this CommonConfigurator configurator, Action<DBOptions> configOptions = null, DBOptions options = null)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStylesDeliveredMongoDB));

            options = options ?? new DBOptions();
            configOptions?.Invoke(options);

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IStylesDeliveredDB>(() => new StylesDeliveredDB(options.ConnStrName), LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IProductDAL, ProductDAL>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IUserDAL, UserDAL>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }

    public class DBOptions
    {
        public string ConnStrName { get; set; } = "MONGO_StylesDelivered";
    }
}
