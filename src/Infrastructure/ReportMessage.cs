using MimeKit;

namespace EmailWorker.Infrastructure;
internal class ReportMessage
{
    internal static MimeMessage CreateReportMessage(
        string login, 
        string emailToReport,
        string emailSubject,
        string emailText
    )
    {
        MimeMessage message = new ();
        message.From.Add(new MailboxAddress("Worker", login));
        message.To.Add(new MailboxAddress("Dmitry", emailToReport));
        message.Subject = emailSubject;
        message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = string.Format(emailText)
        };
        
        return message;
    }
}