using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.DataAccess.Product;
using SyncSoft.StylesDelivered.DataAccess.User;
using SyncSoft.StylesDelivered.DataFacade.Product;
using SyncSoft.StylesDelivered.DataFacade.User;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStylesDeliveredDF(this CommonConfigurator configurator)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStylesDeliveredDF));

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IProductDF, ProductDF>(LifeCycleEnum.Singleton);
                    ObjectContainer.Register<IUserDF, UserDF>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
