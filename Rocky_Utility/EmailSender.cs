using System.Threading.Tasks;
using EmailService;

namespace Rocky_Utility
{
    public class EmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        private readonly ISender _emailSender;

        public EmailSender(ISender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            // TODO: передавать в to параметр email
            var message = new Message(new string[] { "davidtri2013@yandex.ru" }, subject, body, null);
            await _emailSender.SendEmailAsync(message);
        }
    }
}
