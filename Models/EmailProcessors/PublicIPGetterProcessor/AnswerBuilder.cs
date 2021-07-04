using EmailWorker.Models;
using EmailWorker.Models.EmailProcessors.EmailProcessorBase;
using MimeKit;

namespace EmailWorker.Controllers.EmailProcessors.PublicIPGetterProcessor
{
    public class AnswerBuilder
    {
        public static MimeMessage BuildAnswer(IEmailProcessorModel model)
        {
            MimeMessage message = FromToBuilder.BuildFromTo(model);
            message.Subject = "Ip By Email Project";
            string myIP = PublicIPGetter.GetPublicIP();
            message.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = string.Format("The current IP of the computer is {0}", myIP)
            };
            return message;
        }
    }
}