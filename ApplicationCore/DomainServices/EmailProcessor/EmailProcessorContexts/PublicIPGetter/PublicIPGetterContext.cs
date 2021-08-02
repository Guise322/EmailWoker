using System.Collections.Generic;
using EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.MarkAsSeen;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.ServiceContexts;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.PublicIPGetter
{
    public class PublicIPGetterContext : MarkAsSeenContext, IPublicIPGetterContext
    {
        private IMessageGetter MessageGetter { get; set; }
        private IPublicIPGetter IPGetter { get; set; }
        private string SearchedEmail { get; }
        public PublicIPGetterContext(
            string searchedEmail,
            IMessageGetter messageGetter,
            IPublicIPGetter ipGetter,
            IAnswerSender answerSender,
            IUnseenMessagesGetter unseenMessagesGetter,
            IProcessedMessagesHandler processedMessagesHandler,
            IClientConnector clientConnector) :
            base(
                answerSender,
                unseenMessagesGetter,
                processedMessagesHandler,
                clientConnector) =>
            (SearchedEmail, MessageGetter, IPGetter) =
            (searchedEmail, messageGetter, ipGetter);
        public override List<object> ProcessMessages(List<object> messages)
        {
            bool isUniqueId = messages is IList<UniqueId>;
            foreach (var message in messages)
            {
                MimeMessage takenMessage = isUniqueId ? MessageGetter.GetMessage((UniqueId)message) :
                    MessageGetter.GetMessage((int)message);
                string rawEmailFrom = takenMessage.From.ToString();

                string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);

                if (emailFrom == SearchedEmail)
                {
                    return new List<object>(1) { message };
                }
            }
            return null;
        }
        public override MimeMessage BuildAnswerMessage(List<object> messages,
            MimeMessage messageWithFromTo)
            {
                string myIP = IPGetter.GetPublicIP();
                return AnswerMessageBuilder.BuildAnswerMessage(messageWithFromTo, myIP);
            }
    }
}