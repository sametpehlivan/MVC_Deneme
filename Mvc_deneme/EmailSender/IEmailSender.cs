using System.Threading.Tasks;

namespace Mvc_deneme.EmailSender
{
    public interface IEmailSender
    {
        Task SendMailAsync(string email,string subject,string htmlMessage);
    }
}
