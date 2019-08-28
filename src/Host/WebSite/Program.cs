using SyncSoft.App;
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
                    o.ProjectName = "StylesDelivered";
                })
                .UseWebSiteApiClient()
                .UseStylesDeliveredDomain()
                .UseStylesDeliveredMongoDB()
                .UseStylesDeliveredDF()
                .UseJsonConfiguration()
                .Start();

            QuartzHost.Run<Startup>(args);
        }
    }
}
