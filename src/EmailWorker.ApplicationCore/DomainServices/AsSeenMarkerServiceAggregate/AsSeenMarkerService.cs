using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public class AsSeenMarkerService : EmailInboxServiceBase, IAsSeenMarkerService
    {
        private readonly string _emailToReport  = "guise322@yandex.ru";
        private readonly IAsSeenMarker _asSeenMarker;

        public AsSeenMarkerService(
            IAsSeenMarker AsSeenMarker,
            IReportSender reportSender,
            IUnseenMessageIDListGetter getterOfUnseenMessageIDs,
            IClientConnector clientConnector
        ) : base (reportSender, getterOfUnseenMessageIDs, clientConnector) =>
            _asSeenMarker = AsSeenMarker;
        public async Task<ServiceStatus> ProcessEmailInbox()
        {
            IList<UniqueId> messages = await GetMessageIDsFromEmail();
            
            bool analyzeResult = MessageAnalyser.AnalyseMessages(messages);
            
            if (!analyzeResult)
            {
                return new ServiceStatus()
                { ServiceWorkMessage = "The given number of messages is too small." };
            }

            EmailData emailData =
                _asSeenMarker.MarkAsSeen(messages.ToList());

            MimeMessage message = ReportMessage.CreateReportMessage(
                EmailCredentials.Login,
                _emailToReport,
                emailData
            );

            ReportSender.SendReportViaSmtp(message, EmailCredentials);
            
            return new ServiceStatus() 
            { ServiceWorkMessage = "All messages is marked as seen." };
        }
    }
}