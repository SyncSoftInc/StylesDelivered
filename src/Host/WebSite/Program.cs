using SyncSoft.App;
using SyncSoft.App.Securities;
using SyncSoft.ECP.Quartz.Hosting;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.WebSite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Engine.Init(args)
                .UseEcpHostQuickSettings(o =>
                {
                    o.ConfigECPSecurityComponentsOptions = a =>
                    {
                        a.CoreCertProviderType = typeof(ConfigurationCoreCertProvider);
                    };
                })
                .UseMessageQueue()
                .UseDefaultMessageComponents()
                .UseWebSiteApiClient()
                .UseStydDomain()
                .UseStydRedis()
                .UseStydMySql()
                .UseStydDF()
                .UseStydShared()
                .UseStydOffice()
                .UseGRPC()
                .UseAliyun()
                .UseEcomApi()
                .UseHandlebars()
                .UseJsonConfiguration()
                .Start();

            var host = QuartzHost
                .CreateHostBuilder<Startup>(args)
                .Build();

            await QuartzHost.RunAsync(host).ConfigureAwait(false);

            Engine.Stop();
        }
    }
}
