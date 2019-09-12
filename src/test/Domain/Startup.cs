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
                .UseStydDomain()
                .UseStydRedis()
                .UseStydMySql()
                .UseStydDF()
                .UseStydShared()
                .UseGRPC()
                .UseAliyun()
                .UseJsonConfiguration()
                .Start();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Engine.Stop();
        }
    }
}