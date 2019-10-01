using Mail;
using SyncSoft.App.Components;
using SyncSoft.App.ViewEngine;
using SyncSoft.StylesDelivered.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Mail
{
    public class EmailService : IEmailService
    {
        private static readonly Lazy<IEmailSender> _lazyEmailSender = ObjectContainer.LazyResolve<IEmailSender>();
        private IEmailSender EmailSender => _lazyEmailSender.Value;

        private static readonly Lazy<IViewEngine> _lazyViewEngine = ObjectContainer.LazyResolve<IViewEngine>();
        private IViewEngine ViewEngine => _lazyViewEngine.Value;

        public async Task<string> OrderSendAsync(string title, IList<OrderItemDTO> items)
        {
            var item = items.FirstOrDefault();
            var body = await ViewEngine.RenderAsync("StydApproveEmail", () => Task.FromResult<OrderItemDTO>(item)).ConfigureAwait(false);

            var mailMsg = new MailMSG
            {
                Subject = $"Order {title} - {item.OrderNo}",
                Body = body
            };
            mailMsg.To.Add("jonathan.poon@syncsoftinc.com");

            return await EmailSender.EnqueueAsync(mailMsg).ConfigureAwait(false);
        }
    }
}
