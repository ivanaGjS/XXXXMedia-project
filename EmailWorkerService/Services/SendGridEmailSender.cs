using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailWorkerService.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly string _apiKey;
        private readonly EmailAddress _from;
        private readonly ILogger<SendGridEmailSender> _logger;

        public SendGridEmailSender(IConfiguration config, ILogger<SendGridEmailSender> logger)
        {
            _apiKey = config["SendGrid:ApiKey"] ?? throw new InvalidOperationException("SendGrid:ApiKey missing");
            _from = new EmailAddress(config["SendGrid:FromEmail"] ?? "no-reply@xxxxmedia.local", config["SendGrid:FromName"] ?? "XXXXMedia");
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var client = new SendGridClient(_apiKey);
            var msg = MailHelper.CreateSingleEmail(_from, new EmailAddress(to), subject, plainTextContent: null, htmlContent: htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
                _logger.LogError("SendGrid error for {to}: {status}", to, response.StatusCode);
            else
                _logger.LogInformation("Sent email to {to}", to);
        }
    }
}