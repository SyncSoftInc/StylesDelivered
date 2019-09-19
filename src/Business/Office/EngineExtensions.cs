using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.Domain.Order;
using SyncSoft.StylesDelivered.Office.Order;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStydOffice(this CommonConfigurator configurator)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStydOffice));

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IOrderExporter, EPPlusOrderExporter>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
