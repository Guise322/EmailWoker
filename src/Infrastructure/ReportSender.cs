using EmailWorker.Application;
using EmailWorker.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailWorker.Infrastructure;
public class ReportSender : IReportSender
{
    private const int SmtpPort = 465;

    public void SendReportViaSmtp(
        string emailToReport,
        string emailSubject,
        string emailText,
        EmailCredentials emailCredentials
    )
    {
        MimeMessage message = ReportMessage.CreateReportMessage(
            login: emailCredentials.Login,
            emailToReport: emailToReport,
            emailSubject: emailSubject,
            emailText: emailText
        );
        using var client = new SmtpClient();
        client.Connect(emailCredentials.MailServer, SmtpPort, emailCredentials.Ssl);
        client.Authenticate(emailCredentials.Login, emailCredentials.Password);
        client.Send(message);
        client.Disconnect(true);
    }
}