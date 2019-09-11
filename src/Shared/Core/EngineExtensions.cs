using SyncSoft.App.Components;
using SyncSoft.App.Configurations;
using SyncSoft.App.EngineConfigs;
using SyncSoft.App.GRPC;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseStydShared(this CommonConfigurator configurator)
        {
            Engine.PreventDuplicateRegistration(nameof(UseStydShared));

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register(() =>
                    {
                        var endpoint = ObjectContainer.Resolve<IConfigProvider>().GetValue<string>("Services:Logistics");

                        var channel = ObjectContainer.Resolve<IChannelFactory>().Create(endpoint);
                        return new Warehouse.Inventory.InventoryClient(channel);
                    }, LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
