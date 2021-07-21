using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase
{
    public class EmailProcessor : IEmailProcessor
    {
        protected EmailEntity EmailEntity { get; }
        protected string MyEmail { get; } = "guise322@yandex.ru";
        private EmailServicesBase Services { get; }
        public EmailProcessor(EmailEntity emailEntity, EmailServicesBase services)
        {
            EmailEntity = emailEntity;
            Services = services;
        }
        public Task<List<object>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            return Services.UnseenMessagesGetter.GetUnseenMessagesAsync(emailCredentials);
        }
        public virtual List<object> ProcessMessages(List<object> messages)
        {
            return MessagesProcessor.ProcessMessages(messages);
        }
        public virtual void HandleProcessedMessages(List<object> messages)
        {
            Services.ProcessedMessagesHandler.HandleProcessedMessages(messages);
        }
        public virtual MimeMessage BuildAnswerMessage(List<object> messages) =>
            AnswerMessageBuilder.BuildAnswerMessage(messages, EmailEntity.EmailCredentials,
                MyEmail);
        public void SendAnswerBySmtp(MimeMessage message) =>
            Services.AnswerSender.SendAnswerBySmtp(message, EmailEntity.EmailCredentials);
        public async Task ProcessEmailBoxAsync()
        {
            Services.ClientConnector.ConnectClient(EmailEntity.EmailCredentials);
            List<object> unseenMessages = await GetUnseenMessagesAsync(EmailEntity.EmailCredentials);
            Services.ClientConnector.DisconnectClient();
            List<object> processedMessages = ProcessMessages(unseenMessages);
            if(processedMessages != null)
            {
                HandleProcessedMessages(unseenMessages);
                MimeMessage answerMessage = BuildAnswerMessage(unseenMessages);
                SendAnswerBySmtp(answerMessage);
            }
        }
    }
}