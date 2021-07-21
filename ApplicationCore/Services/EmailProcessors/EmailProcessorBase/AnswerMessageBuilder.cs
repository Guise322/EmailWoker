using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase
{
    public class AnswerMessageBuilder
    {
        public static MimeMessage BuildAnswerMessage(List<object> messages,
            EmailCredentials emailCredentials, string emailAddress)
        {
            MimeMessage message = FromToBuilder.BuildFromTo(emailCredentials, emailAddress);
            message.Subject = "The count of messages marked as seen";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The count of seen messages equals {0}", messages.Count)
            };
            return message;
        }
    }
}