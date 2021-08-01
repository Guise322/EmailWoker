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
        public IAnswerSender AnswerSender { get; set; }
        public IUnseenMessagesGetter UnseenMessagesGetter { get; set; }
        public IProcessedMessagesHandler ProcessedMessagesHandler { get; set; }
        public IClientConnector ClientConnector { get; set; }
        public async Task<List<object>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            ClientConnector.ConnectClient(emailCredentials);
            List<object> messages = await UnseenMessagesGetter
                .GetUnseenMessagesAsync(emailCredentials);
            ClientConnector.DisconnectClient();
            return messages;
        }
        public virtual List<object> ProcessMessages(List<object> messages)
        {
            return MessagesProcessor.ProcessMessages(messages);
        }
        public virtual void HandleProcessedMessages(List<object> messages)
        {
            ProcessedMessagesHandler.HandleProcessedMessages(messages);
        }
        public virtual MimeMessage BuildAnswerMessage(List<object> messages,
            MimeMessage messageWithFromTo)
        {
            return AnswerMessageBuilder.BuildAnswerMessage(messages, messageWithFromTo);
        }
        public void SendAnswerBySmtp(MimeMessage message,
            EmailCredentials emailCredentials) =>
            AnswerSender.SendAnswerBySmtp(message, emailCredentials);
    }
}