using SyncSoft.App.Components;
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
                        var channel = ObjectContainer.Resolve<IChannelFactory>().Create("localhost:9999");
                        return new Warehouse.Inventory.InventoryClient(channel);
                    }, LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
