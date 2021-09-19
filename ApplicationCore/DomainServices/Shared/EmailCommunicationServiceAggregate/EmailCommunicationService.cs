using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.Shared.EmailCommunicationServiceAggregate
{
    public class EmailCommunicationService
    {
        private IClientConnector ClientConnector { get; set; }
        private IGetterOfUnseenMessages GetterOfUnseenMessages { get; set; }
        private IReportSender ReportSender { get; set; }

        public EmailCommunicationService(
            IClientConnector clientConnector,
            IGetterOfUnseenMessages getterOfUnseenMessages,
            IReportSender reportSender) =>

            (ClientConnector, GetterOfUnseenMessages, ReportSender) =
            (clientConnector, getterOfUnseenMessages, reportSender);
    
        protected Task<IList<UniqueId>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            ClientConnector.ConnectClient(emailCredentials);
            return GetterOfUnseenMessages.GetUnseenMessagesAsync(emailCredentials);
        }

        protected void SendReportViaSmtp( MimeMessage message,
            EmailCredentials emailCredentials) =>
            ReportSender.SendReportViaSmtp(message, emailCredentials);
    }
}