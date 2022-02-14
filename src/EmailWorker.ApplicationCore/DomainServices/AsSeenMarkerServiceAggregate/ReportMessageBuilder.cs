using EmailWorker.ApplicationCore.DomainServices.Shared;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
public static class ReportMessageBuilder
{
    public static MimeMessage BuildReportMessage(EmailCredentials emailCredentials,
        string emailAddress,
        EmailData emailData)
    {
        MimeMessage messageWithFromTo = FromToBuilder.BuildFromTo(emailCredentials, emailAddress);
        messageWithFromTo.Subject = emailData.EmailSubject;
        messageWithFromTo.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = string.Format(emailData.EmailText)
        };

        return messageWithFromTo;
    }
}