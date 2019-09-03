using NUnit.Framework;
using SyncSoft.App;

namespace DomainTest
{
    [SetUpFixture]
    public class Startup
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Engine.Init()
                .UseEcpHostQuickSettings(o =>
                {
                    o.ProjectName = "styd";
                })
                .UseStylesDeliveredDomain()
                .UseStylesDeliveredRedis()
                .UseStylesDeliveredMySql()
                .UseStylesDeliveredDF()
                .UseAliyun()
                .UseJsonConfiguration()
                .Start();
        }
    }
}