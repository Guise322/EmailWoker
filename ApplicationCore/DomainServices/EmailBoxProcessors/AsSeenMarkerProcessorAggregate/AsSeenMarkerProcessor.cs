using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxProcessorAggregate;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.AsSeenMarkerAggregate
{
    public class AsSeenMarkerProcessor : IAsSeenMarkerProcessor
    {
        private IAnswerSender AnswerSender { get; set; }
        private IGetterOfUnseenMessages UnseenMessagesGetter { get; set; }
        private IHandlerOfAsSeenMarkerMessages ProcessedMessagesHandler { get; set; }
        private IClientConnector ClientConnector { get; set; }
        public AsSeenMarkerProcessor(
            IAnswerSender answerSender,
            IGetterOfUnseenMessages unseenMessagesGetter,
            IHandlerOfAsSeenMarkerMessages processedMessagesHandler,
            IClientConnector clientConnector) => 
            (AnswerSender, UnseenMessagesGetter, ProcessedMessagesHandler, ClientConnector) = 
            (answerSender, unseenMessagesGetter, processedMessagesHandler, clientConnector);

        public async Task<List<object>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            ClientConnector.ConnectClient(emailCredentials);
            List<object> messages = await UnseenMessagesGetter
                .GetUnseenMessagesAsync(emailCredentials);
            ClientConnector.DisconnectClient();
            return messages;
        }
        public virtual List<object> ProcessMessages(List<object> messages) =>
            MessagesProcessor.ProcessMessages(messages);
        public virtual (string emailText, string emailSubject) HandleProcessedMessages(
            List<object> messages)
        {
            return ProcessedMessagesHandler.HandleProcessedMessages(messages);
        }
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