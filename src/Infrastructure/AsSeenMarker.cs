using MailKit;
using MailKit.Net.Imap;
using EmailWorker.Application;
using EmailWorker.Application.Interfaces;

namespace EmailWorker.Infrastructure;

public class AsSeenMarker : IAsSeenMarker
{
    protected readonly IImapClient _client;

    public AsSeenMarker(IImapClient client)
    {
        _client = client;
    }

    public EmailData MarkAsSeen(List<UniqueId> messages)
    {
        foreach (var message in messages)
        {
            _client.Inbox.AddFlags(message, MessageFlags.Seen, true);
        }
        
        return new EmailData()
        {
            EmailSubject = "Mark As Seen Service",
            EmailText = "The count of messages marked as seen " +
                messages.Count.ToString()
        };
    }
}