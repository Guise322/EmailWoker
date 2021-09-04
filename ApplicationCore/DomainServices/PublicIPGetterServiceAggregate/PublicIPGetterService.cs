using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate
{
    public class PublicIPGetterService : AsSeenMarkerService, IPublicIPGetterService
    {
        private IMessageGetter MessageGetter { get; set; }
        private IHandlerOfPublicIPGetterMessages PublicIPGetterMessagesHandler
            { get; set; }
        private string SearchedEmail { get; } = "guise322@ya.ru";
        public PublicIPGetterService(
            ILogger<PublicIPGetterService> logger,
            IMessageGetter messageGetter,
            IAnswerSender answerSender,
            IGetterOfUnseenMessages unseenMessagesGetter,
            IHandlerOfPublicIPGetterMessages handlerOfProcessedMessages,
            IClientConnector clientConnector) :
            base(
                logger,
                answerSender,
                unseenMessagesGetter,
                handlerOfProcessedMessages,
                clientConnector) =>

            MessageGetter = messageGetter;
        public override IList<UniqueId> ProcessMessages(IList<UniqueId> messages)
        {
            UniqueId searchedMessageID = messages.FirstOrDefault(message => 
            {
                MimeMessage messageFromBox = MessageGetter.GetMessage(message);
                string rawEmailFrom = messageFromBox.From.ToString();

                string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);

                return emailFrom == SearchedEmail;
            });

            if (searchedMessageID != default)
            {
                _logger.LogInformation("The request is detected.");
                return new List<UniqueId>(1) { searchedMessageID };
            }

            _logger.LogInformation("The request is not detected.");
            return null;
        }
        public override (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages) =>

            PublicIPGetterMessagesHandler.HandleProcessedMessages(messages);

        public override MimeMessage BuildAnswerMessage(
            EmailCredentials emailCredentials,
            string myEmail,
            string emailSubject,
            string messageText) =>

            AnswerMessageBuilder.BuildAnswerMessage(
                emailCredentials,
                myEmail,
                emailSubject,
                messageText);
        }
}