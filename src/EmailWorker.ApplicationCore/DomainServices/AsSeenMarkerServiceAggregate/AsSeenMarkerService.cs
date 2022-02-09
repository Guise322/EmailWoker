using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.Shared;
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
        private readonly string myEmail  = "guise322@yandex.ru";
        private IReportSender ReportSender { get; set; }
        private IGetterOfUnseenMessageIDs GetterOfUnseenMessages { get; set; }
        private IHandlerOfAsSeenMarkerMessages HandlerOfProcessedMessages { get; set; }
        private IClientConnector ClientConnector { get; set; }

        public AsSeenMarkerService(ILogger<AsSeenMarkerService> logger,
            IReportSender reportSender,
            IGetterOfUnseenMessageIDs getterOfUnseenMessages,
            IHandlerOfAsSeenMarkerMessages handlerOfProcessedMessages,
            IClientConnector clientConnector) =>

            (ReportSender, GetterOfUnseenMessages, HandlerOfProcessedMessages,
                ClientConnector) = 
            (reportSender, getterOfUnseenMessages, handlerOfProcessedMessages,
                clientConnector);

        public async Task<ServiceStatus> ProcessEmailInbox(EmailCredentials emailCredentials)
        {
            //TO DO: add logging.

            IList<UniqueId> messages = 
                await MessageIDsFromEmailGetter.GetMessageIDsFromEmail(ClientConnector,
                    GetterOfUnseenMessages,
                    emailCredentials);
            
            IList<UniqueId> processedMessages;

            try
            {
                processedMessages = AnalyzerOfMessages.AnalyzeMessages(messages);
            }
            catch (ArgumentException)
            {
                return new ServiceStatus() 
                { ServiceWorkMessage = "The service did not get the needed number of messages."};
            }

            (string emailText, string emailSubject) =
                HandlerOfProcessedMessages.HandleProcessedMessages(processedMessages);

            if(emailText == null)
            {
                return new ServiceStatus() 
                { ServiceWorkMessage = "Getting a chunk of unseen messages succeeds." };
            }

            MimeMessage message = ReportMessageBuilder.BuildReportMessage(emailCredentials,
                myEmail,
                emailSubject,
                emailText);

            ReportSender.SendReportViaSmtp(message, emailCredentials);
            
            return new ServiceStatus() 
            { ServiceWorkMessage = "All messages is marked as seen." };
        }
    }
}