using EmailWorker.Application.Interfaces;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;
using EmailWorker.Domain;
using MailKit;
using MimeKit;

namespace EmailWorker.Application;

internal class PublicIPGetterService : IPublicIPGetterService
{
    private const string _searchedEmail = "guise322@ya.ru";
    private readonly IPublicIPGetter _publicIPGetter;
    private readonly IMessageGetter _messageGetter;
    private readonly IAsSeenMarker _asSeenMarker;
    private readonly IReportSender _reportSender;
    private readonly IUnseenMessageIdGetter _unseenMessageIdGetter;
    private readonly IClientConnector _clientConnector;
    
    public PublicIPGetterService(
        IClientConnector clientConnector,
        IUnseenMessageIdGetter unseenMessageIdGetter,
        IMessageGetter messageGetter,
        IAsSeenMarker asSeenMarker,
        IPublicIPGetter publicIPGetter,
        IReportSender reportSender
    )
    {
        _publicIPGetter = publicIPGetter;
        _messageGetter = messageGetter;
        _asSeenMarker = asSeenMarker;
        _reportSender = reportSender;
        _unseenMessageIdGetter = unseenMessageIdGetter;
        _clientConnector = clientConnector;
    }

    public async Task<string> ProcessEmailInbox(EmailCredentials emailCredentials)
    {
        try
        {
            return await ProcessEmailInboxPrivate(emailCredentials);
        }
        catch(FormatException)
        {
            return "The request message has invalid format of its autor string";
        }
    }

    private async Task<string> ProcessEmailInboxPrivate(EmailCredentials emailCredentials)
    {
        _clientConnector.ConnectClient(emailCredentials);
        IList<UniqueId> messageIds =
            await _unseenMessageIdGetter.GetUnseenMessageIds();
        
        MimeMessage mimeMessage;

        var searchedMessageIDs = messageIds.Where(o =>
        {
            mimeMessage = _messageGetter.GetMessage(o);

            return RequestMessageSearcher.IsRequestMessage(
                rawEmailFrom: mimeMessage.From.ToString(),
                searchedEmail: _searchedEmail);
        }).ToList();

        if (searchedMessageIDs.Count == 0)
        {
            _clientConnector.DisconnectClient();
            return "A request is not found";
        }

        EmailData emailData = _publicIPGetter.GetPublicIP();
        _asSeenMarker.MarkAsSeen(searchedMessageIDs);
        _clientConnector.DisconnectClient();

        _reportSender.SendReportViaSmtp(
            emailToReport: _searchedEmail,
            emailSubject: emailData.EmailSubject,
            emailText: emailData.EmailText,
            emailCredentials: emailCredentials
        );

        return "A request is detected, and the current IP address is sent";
    }
}