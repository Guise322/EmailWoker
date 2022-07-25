using EmailWorker.Application.Interfaces;
using EmailWorker.Application.Interfaces.EmailBoxServiceAggregate;
using MailKit;
using MimeKit;

namespace EmailWorker.Application;

internal class AsSeenMarkerService : IAsSeenMarkerService
{
    private const string _emailToReport  = "guise322@yandex.ru";
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

        bool analyzeResult =
            MessageCountValidator.IsMessageCountValid(messages.Count);

        if (!analyzeResult)
        {
            return "The given number of messages is too small.";
        }

        EmailData emailData =
            _asSeenMarker.MarkAsSeen(messages.ToList());

        _clientConnector.DisconnectClient();

        MimeMessage message = ReportMessage.CreateReportMessage(
            emailCredentials.Login,
            _emailToReport,
            emailData
        );

        _reportSender.SendReportViaSmtp(message, emailCredentials);

        return "All messages is marked as seen.";
    }
}