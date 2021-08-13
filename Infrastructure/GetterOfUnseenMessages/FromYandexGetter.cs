using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailProcessor.GetterOfUnseenMessages
{
    public class FromYandexGetter : IGetter
    {
        private ImapClient Client { get; }
        public FromYandexGetter(ImapClient client)
        {
            Client = client;
        }
        public async Task<List<object>> GetUnseenMessagesAsync()
        {
            await Client.Inbox.OpenAsync(FolderAccess.ReadWrite);
            IList<IMessageSummary> messageSummaries =
                await Client.Inbox.FetchAsync(0, Client.Inbox.Count, MessageSummaryItems.All);
            List<object> indexes = new();
            foreach (var item in messageSummaries)
            {
                indexes.Add(item.Index);
            }
            
            return indexes;
        }
    }
}