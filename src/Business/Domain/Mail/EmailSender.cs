using Mail;
using SyncSoft.App.Components;
using System;
using System.Threading.Tasks;
using static Mail.MailService;

namespace SyncSoft.StylesDelivered.Domain.Mail
{
    public class EmailSender : IEmailSender
    {
        private static readonly Lazy<MailServiceClient> _lazyMailService = ObjectContainer.LazyResolve<MailServiceClient>();
        private MailServiceClient MailService => _lazyMailService.Value;

        public async Task<string> SendAsync(MailMSG mailMSG)
        {
            var mr = await MailService.EnqueueAsync(mailMSG);
            return mr.MsgCode;
        }
    }
}
