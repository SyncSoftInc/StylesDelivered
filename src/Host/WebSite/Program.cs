using SyncSoft.App;
using SyncSoft.App.Securities;
using SyncSoft.ECP.Quartz.Hosting;

namespace SyncSoft.StylesDelivered.WebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Engine.Init(args)
                .UseEcpHostQuickSettings(o =>
                {
                    o.ProjectName = "styd";

                    o.ConfigECPSecurityComponentsOptions = a =>
                    {
                        a.CoreCertProviderType = typeof(ConfigurationCoreCertProvider);
                    };
                })
                .UseMessageQueue()
                .UseDefaultMessageComponents()
                .UseWebSiteApiClient()
                .UseStylesDeliveredDomain()
                .UseStylesDeliveredRedis()
                .UseStylesDeliveredMySql()
                .UseStylesDeliveredDF()
                .UseAliyun()
                .UseJsonConfiguration()
                .Start();

            QuartzHost.Run<Startup>(args);
        }
    }
}
