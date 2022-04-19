//Some of the below lines of code is got from the Github issue:
//https://github.com/jstedfast/MailKit/issues/266

using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace EmailWorker.Infrastructure;

public class UnseenMessageIDListGetter : IUnseenMessageIDListGetter
{
    private IImapClient Client { get; }
    public UnseenMessageIDListGetter(IImapClient client) => Client = client;
    public async Task<IList<UniqueId>> GetUnseenMessageIDsAsync(EmailCredentials emailCredentials)
    {
        Client.Inbox.Open(FolderAccess.ReadWrite);

        IList<UniqueId> unseenMessages = await Client.Inbox.SearchAsync(SearchQuery.NotSeen);

        return unseenMessages;
    }
}