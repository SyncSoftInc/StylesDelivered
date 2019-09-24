using Mail;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Mail
{
    public interface IEmailSender
    {
        Task<string> SendAsync(MailMSG mailMSG);
    }
}
