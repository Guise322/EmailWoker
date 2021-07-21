using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;
using MimeKit;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.PublicIPGetterProcessor
{
    public class AnswerBuilder
    {
        public static MimeMessage BuildAnswerMessages(EmailCredentials emailCredentials,
            string emailAddress, IPublicIPGetter ipGetter)
        {
            MimeMessage message = FromToBuilder.BuildFromTo(emailCredentials, emailAddress);
            message.Subject = "Ip By Email Project";
            string myIP = ipGetter.GetPublicIP();
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The current IP of the computer is {0}", myIP)
            };
            
            return message;
        }
    }
}