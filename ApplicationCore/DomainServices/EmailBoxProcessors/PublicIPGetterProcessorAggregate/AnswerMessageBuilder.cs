using EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.Shared;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.PublicIPGetterAggregate
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