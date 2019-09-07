using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.DataAccess;
using SyncSoft.StylesDelivered.DataAccess.Common;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.MySql;
using SyncSoft.StylesDelivered.MySql.Common;
using SyncSoft.StylesDelivered.MySql.Product;
using SyncSoft.StylesDelivered.MySql.ShoppingCart;
using SyncSoft.StylesDelivered.MySql.User;
using System;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStylesDeliveredMySql(this CommonConfigurator configurator, Action<DBOptions> configOptions = null, DBOptions options = null)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStylesDeliveredMySql));

            options = options ?? new DBOptions();
            configOptions?.Invoke(options);

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IMasterDB>(() => new MasterDB(options.DBConnStrName), LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IProductDAL, ProductDAL>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IProductItemDAL, ProductItemDAL>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IUserDAL, UserDAL>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IShoppingCartDAL, ShoppingCartDAL>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<ICommonDAL, CommonDAL>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
