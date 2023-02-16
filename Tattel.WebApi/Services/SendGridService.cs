using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Tattel.WebApi.Services
{
    public static class SendGridService
    {
        public static async Task SendEmail(string subject, string content, string emailAddress)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("hello@tattel.co.uk", "Verification Mail");
            var to = new EmailAddress(emailAddress);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", content);
            var response = await client.SendEmailAsync(msg);
            if (response == null || (int)response.StatusCode < 200 || (int)response.StatusCode > 299)
                throw new Exception("Error: SendEmail To: " + emailAddress + " Content: " + content);
        }
    }
    
}
