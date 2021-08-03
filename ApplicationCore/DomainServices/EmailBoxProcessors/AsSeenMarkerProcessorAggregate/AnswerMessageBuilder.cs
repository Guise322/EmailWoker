using System.Collections.Generic;
using EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.Shared;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.AsSeenMarkerAggregate
{
    public class AnswerMessageBuilder
    {
        public static MimeMessage BuildAnswerMessage(
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