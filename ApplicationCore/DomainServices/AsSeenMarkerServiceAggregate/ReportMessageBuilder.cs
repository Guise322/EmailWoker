using System.Collections.Generic;
using EmailWorker.ApplicationCore.DomainServices.Shared;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public static class ReportMessageBuilder
    {
        public static MimeMessage BuildReportMessage(
            EmailCredentials emailCredentials,
            string emailAdress,
            string emailSubject,
            string messageText)
        {
            MimeMessage messageWithFromTo = FromToBuilder.BuildFromTo(emailCredentials, emailAdress);
            messageWithFromTo.Subject = emailSubject;
            messageWithFromTo.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format(messageText)
            };

            return messageWithFromTo;
        }
    }
}