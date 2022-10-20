using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Mvc_deneme.EmailSender
{
    public class SmtpEmailSender : IEmailSender
    {
        private string _host;
        private int _port;
        private bool _enableSSL;
        private string _UserName;
        private string _pass;

        public SmtpEmailSender(string host,int port,bool enableSSL,string UserName,string pass)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSL;
            _UserName = UserName;
            _pass = pass;
        }
        public Task SendMailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_UserName, _pass),
                EnableSsl = _enableSSL
            };
            var mail = new MailMessage(_UserName, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };
            return client.SendMailAsync(mail);
        }
    }
}
