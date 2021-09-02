using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public class AsSeenMarkerService : IAsSeenMarkerService
    {
        protected readonly ILogger _logger;
        private IAnswerSender AnswerSender { get; set; }
        private IGetterOfUnseenMessages UnseenMessagesGetter { get; set; }
        private IHandlerOfAsSeenMarkerMessages ProcessedMessagesHandler { get; set; }
        private IClientConnector ClientConnector { get; set; }
        public AsSeenMarkerService(
            ILogger<AsSeenMarkerService> logger,
            IAnswerSender answerSender,
            IGetterOfUnseenMessages unseenMessagesGetter,
            IHandlerOfAsSeenMarkerMessages processedMessagesHandler,
            IClientConnector clientConnector) =>

            (_logger, AnswerSender, UnseenMessagesGetter, ProcessedMessagesHandler, ClientConnector) = 
            (logger, answerSender, unseenMessagesGetter, processedMessagesHandler, clientConnector);

        public async Task<IList<UniqueId>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            ClientConnector.ConnectClient(emailCredentials);
            return await UnseenMessagesGetter.GetUnseenMessagesAsync(emailCredentials);
        }
        public virtual IList<UniqueId> ProcessMessages(IList<UniqueId> messages) =>

           MessagesProcessor.ProcessMessages(_logger, messages);
            
        public virtual (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages) =>
            ProcessedMessagesHandler.HandleProcessedMessages(messages);
        public virtual MimeMessage BuildAnswerMessage(
            EmailCredentials emailCredentials,
            string myEmail,
            string emailSubject,
            string messageText) =>
        AnswerMessageBuilder.BuildAnswerMessage(
            emailCredentials,
            myEmail,
            emailSubject,
            messageText);
        public void SendAnswerBySmtp(
            MimeMessage message, EmailCredentials emailCredentials) =>
            AnswerSender.SendAnswerBySmtp(message, emailCredentials);
    }
}