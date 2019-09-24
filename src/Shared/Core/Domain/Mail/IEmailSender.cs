using Mail;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Mail
{
    public interface IEmailSender
    {
        /// <summary>
        /// 将邮件放入发送队列
        /// </summary>
        Task<string> EnqueueAsync(MailMSG mailMSG);
    }
}
