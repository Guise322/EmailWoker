using EmailWorker.Application.Interfaces;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;
using MailKit;
using MimeKit;

namespace EmailWorker.Application;

internal class AsSeenMarkerService : IAsSeenMarkerService
{
    private const string _emailToReport = "guise322@yandex.ru";
    private readonly IAsSeenMarker _asSeenMarker;
    private readonly IClientConnector _clientConnector;
    private readonly IUnseenMessageIdGetter _unseenMessageIdGetter;
    private readonly IReportSender _reportSender;

    public AsSeenMarkerService(
        IAsSeenMarker asSeenMarker,
        IClientConnector clientConnector,
        IUnseenMessageIdGetter unseenMessageIDListGetter,
        IReportSender reportSender
    )
    {
        _asSeenMarker = asSeenMarker;
        _clientConnector = clientConnector;
        _unseenMessageIdGetter = unseenMessageIDListGetter;
        _reportSender = reportSender;
    }

    public async Task<string> ProcessEmailInbox(EmailCredentials emailCredentials)
    {
        _clientConnector.ConnectClient(emailCredentials);

        IList<UniqueId> messages =
            await _unseenMessageIdGetter.GetUnseenMessageIds();

        bool result =
            MessageCountValidator.IsMessageCountValid(messages.Count);

        if (!result)
        {
            _clientConnector.DisconnectClient();
            return "The given number of messages is too small";
        }

        EmailData emailData =
            _asSeenMarker.MarkAsSeen(messages.ToList());

        _clientConnector.DisconnectClient();

        _reportSender.SendReportViaSmtp(
            emailToReport: _emailToReport,
            emailSubject: emailData.EmailSubject,
            emailText: emailData.EmailText,
            emailCredentials: emailCredentials
        );

        return "All messages is marked as seen";
    }
}