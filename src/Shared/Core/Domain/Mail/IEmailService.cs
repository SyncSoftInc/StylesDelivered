using SyncSoft.StylesDelivered.DTO.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncSoft.StylesDelivered.Domain.Mail
{
    public interface IEmailService
    {
        Task<string> OrderSendAsync(string title, IList<OrderItemDTO> items);
    }
}