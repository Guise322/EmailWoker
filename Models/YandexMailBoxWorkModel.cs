using System;
using EmailWorker.Models.Interfaces;
using MailKit.Search;
using EmailWorker.Shared;
using MailKit.Net.Imap;
using MailKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailWorker.Models
{
    public class YandexMailBoxWorkModel : IEmailBoxWorkModel
    {
        private EmailCredentials emailCredentials;
        public YandexMailBoxWorkModel(EmailCredentials emailCredentials)
        {
            this.emailCredentials = emailCredentials;
        }
        public async Task<IList<object>> GetUnseenMessagesFromInboxAsync(ImapClient client)
        {
            client.Connect(emailCredentials.MailServer, emailCredentials.Port, emailCredentials.Ssl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(emailCredentials.Login, emailCredentials.Password);
            client.Inbox.Open(FolderAccess.ReadWrite);
            int messageCount = client.Inbox.Count;
            IList<IMessageSummary> messageSummaries = await client.Inbox.FetchAsync(0, messageCount,
                MessageSummaryItems.All
            );
            List<object> indexes = new();
            foreach (var item in messageSummaries)
            {
                indexes.Add(item.Index);
            }
            return indexes;
        }
    }
}