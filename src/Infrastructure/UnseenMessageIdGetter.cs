//Some of the below lines of code is got from the Github issue:
//https://github.com/jstedfast/MailKit/issues/266

using EmailWorker.Application.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace EmailWorker.Infrastructure;

public class UnseenMessageIdGetter : IUnseenMessageIdGetter
{
    private readonly IImapClient _client;
    public UnseenMessageIdGetter(IImapClient client)
    {
        _client = client;
    }
    
    public async Task<IList<UniqueId>> GetUnseenMessageIds()
    {
        _client.Inbox.Open(FolderAccess.ReadWrite);
        IList<UniqueId> unseenMessages = await _client.Inbox.SearchAsync(SearchQuery.NotSeen);

        return unseenMessages;
    }
}