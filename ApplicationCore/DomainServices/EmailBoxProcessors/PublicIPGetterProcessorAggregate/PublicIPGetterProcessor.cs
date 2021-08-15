using System.Collections.Generic;
using EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.AsSeenMarkerAggregate;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxProcessorAggregate;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.PublicIPGetterAggregate
{
    public class PublicIPGetterProcessor : AsSeenMarkerProcessor, IPublicIPGetterProcessor
    {
        private IMessageGetter MessageGetter { get; set; }
        private IHandlerOfPublicIPGetterMessages PublicIPGetterMessagesHandler
            { get; set; }
        private string SearchedEmail { get; } = "guise322@ya.ru";
        public PublicIPGetterProcessor(
            IMessageGetter messageGetter,
            IAnswerSender answerSender,
            IGetterOfUnseenMessages unseenMessagesGetter,
            IHandlerOfPublicIPGetterMessages handlerOfProcessedMessages,
            IClientConnector clientConnector) :
            base(
                answerSender,
                unseenMessagesGetter,
                handlerOfProcessedMessages,
                clientConnector) =>
            MessageGetter = messageGetter;
        public override IList<UniqueId> ProcessMessages(IList<UniqueId> messages)
        {
            foreach (var message in messages)
            {
                MimeMessage messageFromBox = MessageGetter.GetMessage(message);
                string rawEmailFrom = messageFromBox.From.ToString();

                string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);

                if (emailFrom == SearchedEmail)
                {
                    return new List<UniqueId>(1) { message };
                }
            }
            return null;
        }
        public override (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            return PublicIPGetterMessagesHandler.HandleProcessedMessages(messages);
        }
    }
}