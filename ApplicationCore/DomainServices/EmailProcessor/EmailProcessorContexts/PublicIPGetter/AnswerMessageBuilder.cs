using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.PublicIPGetter
{
    public class AnswerMessageBuilder
    {
        public static MimeMessage BuildAnswerMessage(MimeMessage messageWithFromTo, string myIP)
        {
            messageWithFromTo.Subject = "Ip By Email Project";
            messageWithFromTo.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The current IP of the computer is {0}", myIP)
            };
            
            return messageWithFromTo;
        }
    }
}