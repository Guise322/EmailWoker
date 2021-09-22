using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.Shared.EmailCommunicationServiceAggregate;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public class AsSeenMarkerService : EmailCommunicationService, IAsSeenMarkerService
    {
        private readonly ILogger<AsSeenMarkerService> _logger;
        
        private IHandlerOfAsSeenMarkerMessages ProcessedMessagesHandler { get; set; }
        
        public AsSeenMarkerService(ILogger<AsSeenMarkerService> logger,
            IReportSender reportSender,
            IGetterOfUnseenMessages getterOfUnseenMessages,
            IHandlerOfAsSeenMarkerMessages handlerOfProcessedMessages,
            IClientConnector clientConnector) : 
            base(clientConnector, getterOfUnseenMessages, reportSender) =>

            (_logger, ProcessedMessagesHandler) = 
            (logger, handlerOfProcessedMessages);

        public async Task<IList<UniqueId>> AnalyzeMessages(EmailCredentials emailCredentials)
        {
            IList<UniqueId> messages = await GetUnseenMessagesAsync(emailCredentials);
            return MessagesAnalyzer.AnalyzeMessages(_logger, messages);
        }    
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages) =>
            
            ProcessedMessagesHandler.HandleProcessedMessages(messages);
        
        public void SendReportMessageViaEmail(EmailCredentials emailCredentials,
            string myEmail,
            string emailSubject,
            string messageText)
        {
            MimeMessage message = ReportMessageBuilder.BuildReportMessage(emailCredentials,
                myEmail,
                emailSubject,
                messageText);

            SendReportViaSmtp(message, emailCredentials);
        }
    }
}