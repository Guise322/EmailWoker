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
        IPublicIPGetter publicIPGetter,
        IMessageGetter messageGetter,
        IAsSeenMarker asSeenMarker,
        IReportSender reportSender,
        IUnseenMessageIdGetter unseenMessageIdGetter,
        IClientConnector clientConnector
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
            return "The request message has the invalid format of its autor string.";
        }
        catch(ArgumentException)
        {
            return "The response message has no or invalid address string.";
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
            return "The request is not found.";
        }

        EmailData emailData = _publicIPGetter.GetPublicIP();
        _asSeenMarker.MarkAsSeen(searchedMessageIDs);
        _clientConnector.DisconnectClient();

        MimeMessage message = ReportMessage.CreateReportMessage(
            emailCredentials.Login,
            _searchedEmail,
            emailData
        );

        _reportSender.SendReportViaSmtp(message, emailCredentials);

        return "The request is detected. The current IP address is sent.";
    }
}