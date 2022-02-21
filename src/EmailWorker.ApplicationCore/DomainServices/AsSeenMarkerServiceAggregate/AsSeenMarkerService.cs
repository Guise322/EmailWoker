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
        public EmailCredentials EmailCredentials { get; set; }
        private readonly string myEmail  = "guise322@yandex.ru";
        private IReportSender ReportSender { get; set; }
        private IGetterOfUnseenMessageIDs GetterOfUnseenMessages { get; set; }
        private IHandlerOfAsSeenMarkerMessages HandlerOfProcessedMessages { get; set; }
        private IClientConnector ClientConnector { get; set; }

        public AsSeenMarkerService(
            IReportSender reportSender,
            IGetterOfUnseenMessageIDs getterOfUnseenMessages,
            IHandlerOfAsSeenMarkerMessages handlerOfProcessedMessages,
            IClientConnector clientConnector) =>

            (ReportSender, GetterOfUnseenMessages, HandlerOfProcessedMessages,
                ClientConnector) = 
            (reportSender, getterOfUnseenMessages, handlerOfProcessedMessages,
                clientConnector);

        public async Task<ServiceStatus> ProcessEmailInbox()
        {
            IList<UniqueId> messages = 
                await MessageIDsFromEmailGetter.GetMessageIDsFromEmail(
                    ClientConnector,
                    GetterOfUnseenMessages,
                    EmailCredentials);
            
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

            EmailData emailData = 
                HandlerOfProcessedMessages.HandleProcessedMessages(processedMessages);

            if(emailData == null)
            {
                return new ServiceStatus()
                { ServiceWorkMessage = "Getting a chunk of unseen messages succeeds." };
            }

            MimeMessage message = ReportMessageFactory.CreateReportMessage(
                EmailCredentials,
                myEmail,
                emailData);

            ReportSender.SendReportViaSmtp(message, EmailCredentials);
            
            return new ServiceStatus() 
            { ServiceWorkMessage = "All messages is marked as seen." };
        }
    }
}