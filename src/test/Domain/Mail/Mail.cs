using NUnit.Framework;
using SyncSoft.App.Components;
using SyncSoft.StylesDelivered.Domain.Mail;
using System;
using System.Threading.Tasks;

namespace Mail
{
    public class Mail
    {
        // *******************************************************************************************************************************
        #region -  Lazy Object(s)  -

        private static readonly Lazy<IEmailSender> _lazyEmailSender = ObjectContainer.LazyResolve<IEmailSender>();
        private IEmailSender EmailSender => _lazyEmailSender.Value;

        #endregion
        // *******************************************************************************************************************************
        #region -  CreateOrder  -

        [Test]
        public async Task Send()
        {
            var msg = new MailMSG
            {
                Subject = "Test Subject",
                Body = "Test Body"
            };
            msg.To.Add("lukiya.chen@syncsoftinc.com");
            var msgCode = await EmailSender.SendAsync(msg).ConfigureAwait(false);
            Assert.IsTrue(msgCode.IsSuccess());
        }

        #endregion
    }
}
