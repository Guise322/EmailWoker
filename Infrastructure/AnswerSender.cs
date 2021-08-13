using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailWorker.Infrastructure.EmailProcessor
{
    public class AnswerSender : IAnswerSender
    {
        public void SendAnswerBySmtp(MimeMessage message, EmailCredentials emailCredentials)
        {
            int smtpPort = 465;

            using var client = new SmtpClient();
            client.Connect(emailCredentials.MailServer, smtpPort, emailCredentials.Ssl);
            client.Authenticate(emailCredentials.Login, emailCredentials.Password);
            client.Send(message);
        }
    }
}