using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.Infrastructure
{
    public class AnswerSender : IAnswerSender
    {
        private readonly ILogger _logger;
        public AnswerSender(ILogger<AnswerSender> logger)
        {
            _logger = logger;
        }
        public void SendAnswerBySmtp(MimeMessage message, EmailCredentials emailCredentials)
        {
            int smtpPort = 465;

            using var client = new SmtpClient();
            client.Connect(emailCredentials.MailServer, smtpPort, emailCredentials.Ssl);
            client.Authenticate(emailCredentials.Login, emailCredentials.Password);
            client.Send(message);

            _logger.LogInformation($"The answer is sended from the {emailCredentials.Login} inbox.");
        }
    }
}