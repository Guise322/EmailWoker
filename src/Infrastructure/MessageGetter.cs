using EmailWorker.Application.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailWorker.Infrastructure;

public class MessageGetter : IMessageGetter
{
    private readonly IImapClient _client;
    public MessageGetter(IImapClient client)
    {
        _client = client;
    }
    public MimeMessage GetMessage(UniqueId id)
    {
        return _client.Inbox.GetMessage(id);
    }
}