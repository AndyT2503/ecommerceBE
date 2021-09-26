using System.Threading.Tasks;

namespace Ecommerce.Application.Services.MailNotifyService
{
    public interface IMailNotifyService
    {
        Task SendMailAsync(string email, object data, string eventCode);
    }
}
