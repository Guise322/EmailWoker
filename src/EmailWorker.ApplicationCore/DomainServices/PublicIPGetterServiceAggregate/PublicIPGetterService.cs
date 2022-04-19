using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;

public class PublicIPGetterService : EmailInboxServiceBase, IPublicIPGetterService
{
    private readonly IPublicIPGetter _publicIPGetter;
    private readonly IRequestMessageSearcher _requestMessageSearcher;
    private readonly string _searchedEmail = "guise322@ya.ru";
    public PublicIPGetterService(
        IPublicIPGetter publicIPGetter,
        IRequestMessageSearcher requestMessageSearcher,
        IReportSender reportSender,
        IUnseenMessageIDListGetter getterOfUnseenMessages,
        IClientConnector clientConnector
    ) : base (reportSender, getterOfUnseenMessages, clientConnector) =>
        (_publicIPGetter, _requestMessageSearcher) = 
        (publicIPGetter, requestMessageSearcher);

    public async Task<ServiceStatus> ProcessEmailInbox()
    {
        IList<UniqueId> messageIDs = await GetUnseenMessageIDsFromEmail();

        try
        {
            List<UniqueId> searchedMessageIDs =
                _requestMessageSearcher.SearchRequestMessage(messageIDs, _searchedEmail);

            ServiceStatus currentStatus = new () { ServiceWorkMessage = "The request is detected." };

            EmailData emailData = _publicIPGetter.GetPublicIP(searchedMessageIDs);

            ClientConnector.DisconnectClient();

            MimeMessage message = ReportMessage.CreateReportMessage(
                EmailCredentials.Login,
                _searchedEmail,
                emailData
            );
            
            ReportSender.SendReportViaSmtp(message, EmailCredentials);

            currentStatus.ServiceWorkMessage += " The current ip address is sent.";

            return currentStatus;
        }
        catch (InvalidOperationException)
        {
            return new ServiceStatus
            { ServiceWorkMessage = "The request is not found." };
        }
    }
}