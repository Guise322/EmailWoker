using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.Infrastructure
{
    public class ReportSender : IReportSender
    {
        private readonly ILogger<ReportSender> _logger;
        public ReportSender(ILogger<ReportSender> logger) =>
            _logger = logger;
        public void SendReportViaSmtp(MimeMessage message, EmailCredentials emailCredentials)
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