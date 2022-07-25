using MimeKit;

namespace EmailWorker.Application;
internal class ReportMessage
{
    public static MimeMessage CreateReportMessage(
        string login, 
        string emailAddress,
        EmailData emailData
    )
    {
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