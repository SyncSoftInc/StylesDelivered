using SyncSoft.App.Components;
using SyncSoft.App.EngineConfigs;
using SyncSoft.StylesDelivered.Aliyun;
using SyncSoft.StylesDelivered.Storage;

namespace SyncSoft.App
{
    public static class EngineExtensions
    {
        public static CommonConfigurator UseAliyun(this CommonConfigurator configurator)
        {
            Engine.PreventDuplicateRegistration(nameof(UseAliyun));

            if (!Engine.IsStarted)
            {
                configurator.Engine.Starting += (o, e) =>
                {
                    ObjectContainer.Register<IStorage, AliyunStorage>(LifeCycleEnum.Singleton);
                };
            }

            return configurator;
        }
    }
}
