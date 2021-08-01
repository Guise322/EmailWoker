using System.Collections.Generic;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.MarkAsSeen
{
    public class AnswerMessageBuilder
    {
        public static MimeMessage BuildAnswerMessage(List<object> messages, MimeMessage messageWithFromTo)
        {
            messageWithFromTo.Subject = "The count of messages marked as seen";
            messageWithFromTo.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The count of seen messages equals {0}", messages.Count)
            };
            return messageWithFromTo;
        }
    }
}