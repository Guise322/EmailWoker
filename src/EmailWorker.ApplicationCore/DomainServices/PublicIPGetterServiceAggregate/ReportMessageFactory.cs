using EmailWorker.ApplicationCore.DomainServices.Shared;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;
public class ReportMessageFactory
{
    public static MimeMessage CreateReportMessage(EmailCredentials emailCredentials, 
        string emailAddress,
        EmailData emailData)
    {
        MimeMessage messageWithFromTo = FromToFormFactory.CreateFromToForm(emailCredentials, emailAddress);
        messageWithFromTo.Subject = emailData.EmailSubject;
        messageWithFromTo.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = string.Format(emailData.EmailText)
        };
        
        return messageWithFromTo;
    }
}