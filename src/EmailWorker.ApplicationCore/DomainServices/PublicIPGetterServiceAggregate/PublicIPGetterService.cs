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
        IUnseenMessageIDListGetter unseenMessageIDListGetter,
        IClientConnector clientConnector
    ) : base (reportSender, unseenMessageIDListGetter, clientConnector) =>
        (_publicIPGetter, _requestMessageSearcher) = 
        (publicIPGetter, requestMessageSearcher);

    public async Task<ServiceStatus> ProcessEmailInbox()
    {
        IList<UniqueId> messageIDs = await GetUnseenMessageIDsFromEmail();

        try
        {
            List<UniqueId> searchedMessageIDs =
                _requestMessageSearcher.SearchRequestMessage(messageIDs, _searchedEmail);

            if (searchedMessageIDs.Count == 0)
            {
                return new ServiceStatus
                { ServiceWorkMessage = "The request is not found." };
            }

            EmailData emailData = _publicIPGetter.GetPublicIP(searchedMessageIDs);

            _clientConnector.DisconnectClient();

            MimeMessage message = ReportMessage.CreateReportMessage(
                EmailCredentials.Login,
                _searchedEmail,
                emailData
            );
            
            _reportSender.SendReportViaSmtp(message, EmailCredentials);

            return new ServiceStatus()
            { ServiceWorkMessage = "The request is detected. The current ip address is sent." };
        }
        catch(FormatException)
        {
            return new ServiceStatus
            { ServiceWorkMessage = "The request message has the invalid format of its autor string." };
        }
        catch(ArgumentException)
        {
            return new ServiceStatus
            { ServiceWorkMessage = "The response message has no or invalid address string." };
        }
    }
}