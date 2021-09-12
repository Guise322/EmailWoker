using EmailWorker.ApplicationCore.DomainServices.Shared;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate
{
    public class AnswerMessageBuilder
    {
        public static MimeMessage BuildAnswerMessage(
            EmailCredentials emailCredentials, 
            string emailAddress,
            string emailSubject,
            string messageText)
        {
            MimeMessage messageWithFromTo = FromToBuilder.BuildFromTo(emailCredentials, emailAddress);
            messageWithFromTo.Subject = emailSubject;
            messageWithFromTo.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format(messageText)
            };
            
            return messageWithFromTo;
        }
    }
}