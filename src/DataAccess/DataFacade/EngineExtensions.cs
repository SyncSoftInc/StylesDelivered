using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.DataAccess.Common;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DataAccess.ShoppingCart;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DataFacade.Common;
using SyncSoft.StylesDelivered.DataFacade.Product;
using SyncSoft.StylesDelivered.DataFacade.ShoppingCart;
using SyncSoft.StylesDelivered.DataFacade.User;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStydDF(this CommonConfigurator configurator)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStydDF));

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IProductDF, ProductDF>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IProductItemDF, ProductItemDF>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IUserDF, UserDF>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IShoppingCartDF, ShoppingCartDF>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<ICommonDF, CommonDF>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
