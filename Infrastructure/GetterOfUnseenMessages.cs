using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

namespace EmailWorker.Infrastructure
{
    public class GetterOfUnseenMessages : IGetterOfUnseenMessages
    {
        private ImapClient Client { get; }
        public GetterOfUnseenMessages(ImapClient client)
        {
            Client = client;
        }
        public async Task<IList<UniqueId>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            Client.Inbox.Open(FolderAccess.ReadWrite);
            
            return await Client.Inbox.SearchAsync(SearchQuery.NotSeen);
        }
    }
}