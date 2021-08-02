using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.ServiceContexts;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.MarkAsSeen
{
    public class MarkAsSeenContext : IMarkAsSeenContext
    {
        private IAnswerSender AnswerSender { get; set; }
        private IUnseenMessagesGetter UnseenMessagesGetter { get; set; }
        private IProcessedMessagesHandler ProcessedMessagesHandler { get; set; }
        private IClientConnector ClientConnector { get; set; }
        public MarkAsSeenContext(
            IAnswerSender answerSender,
            IUnseenMessagesGetter unseenMessagesGetter,
            IProcessedMessagesHandler processedMessagesHandler,
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
        public virtual void HandleProcessedMessages(List<object> messages)
        {
            ProcessedMessagesHandler.HandleProcessedMessages(messages);
        }
        public virtual MimeMessage BuildAnswerMessage(List<object> messages,
            MimeMessage messageWithFromTo) =>
        AnswerMessageBuilder.BuildAnswerMessage(messages, messageWithFromTo);
        public void SendAnswerBySmtp(MimeMessage message,
            EmailCredentials emailCredentials) =>
            AnswerSender.SendAnswerBySmtp(message, emailCredentials);
    }
}