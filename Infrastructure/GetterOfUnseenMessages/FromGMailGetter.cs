using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace EmailWorker.Infrastructure.EmailProcessor.GetterOfUnseenMessages
{
    public class FromGmailGetter : IGetter
    {
        private ImapClient Client { get; }
        public FromGmailGetter(ImapClient client)
        {
            Client = client;
        }
        public async Task<List<object>> GetUnseenMessagesAsync()
        {
            await Client.Inbox.OpenAsync(FolderAccess.ReadWrite);
            SearchResults unseenMessages = await Client.Inbox
                .SearchAsync(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
            return (List<object>) unseenMessages.UniqueIds;
        }
    }
}