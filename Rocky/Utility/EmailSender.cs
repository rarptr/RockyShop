using EmailService;
using IdentityUIServices = Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Rocky.Utility
{
    public class EmailSender : IdentityUIServices.IEmailSender
    {
        
        private readonly EmailService.IEmailSender _emailSender;

        public EmailSender(EmailService.IEmailSender emailSender)
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
