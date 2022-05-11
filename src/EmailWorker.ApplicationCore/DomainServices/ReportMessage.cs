using Ardalis.GuardClauses;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices;
public class ReportMessage
{
    public static MimeMessage CreateReportMessage(
        string login, 
        string emailAddress,
        EmailData emailData
    )
    {
        Guard.Against.NullOrEmpty(login, nameof(login));
        Guard.Against.NullOrEmpty(emailAddress, nameof(emailAddress));
        Guard.Against.NullOrEmpty(emailData.EmailSubject, nameof(emailData.EmailSubject));
        Guard.Against.NullOrEmpty(emailData.EmailText, nameof(emailData.EmailText));

        MimeMessage message = new ();
        message.From.Add(new MailboxAddress("Worker", login));
        message.To.Add(new MailboxAddress("Dmitry", emailAddress));
        message.Subject = emailData.EmailSubject;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = string.Format(emailData.EmailText)
        };
        
        return message;
    }
}