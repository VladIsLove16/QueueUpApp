using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmailServiceAPI.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            // Извлекаем логин и пароль из конфигурации
            string senderEmail = _configuration["Data:email"] ?? throw new InvalidOperationException("Email is not configured.");
            string senderPassword = _configuration["Data:password"] ?? throw new InvalidOperationException("Password is not configured.");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта QueueApp", senderEmail));
            emailMessage.To.Add(new MailboxAddress("", recipientEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.yandex.ru", 465, SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync(senderEmail, senderPassword);
                   string Response =  await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);

                    _logger.LogInformation("ответ: " + Response);
                    _logger.LogInformation($"Email успешно отправлен на {recipientEmail}.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при отправке email на {recipientEmail}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
