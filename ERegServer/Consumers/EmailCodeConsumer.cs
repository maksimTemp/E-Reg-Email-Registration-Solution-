using MassTransit;
using System.Net;
using System.Net.Mail;
using SharedLibrary.Messages;

namespace ERegServer.Consumers
{
    /// <summary>
    /// Consumer for processing EmailCodeMessage messages.
    /// </summary>
    public class EmailCodeConsumer : IConsumer<EmailCodeMessage>
    {
        private readonly ILogger<EmailCodeConsumer> _logger;
        private readonly IConfiguration _configuration;

        public EmailCodeConsumer(ILogger<EmailCodeConsumer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Consume method to process incoming email code messages.
        /// </summary>
        public async Task Consume(ConsumeContext<EmailCodeMessage> context)
        {
            var message = context.Message;

            //Getting SMTP settings from appsettings.json
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var smtpServer = smtpSettings["Server"];
            var smtpPort = int.Parse(smtpSettings["Port"]);
            var smtpUsername = smtpSettings["Username"];
            var smtpPassword = smtpSettings["Password"];

            try
            {
                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = false, // Enable SSL if necessary
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = "Код подтверждения",
                    Body = $"Ваш код подтверждения: {message.GeneratedCode}",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(message.Email);
                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation($"Sending code {message.GeneratedCode} to {message.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending code to {message.Email}: {ex.Message}");
            }

        }

    }
}
