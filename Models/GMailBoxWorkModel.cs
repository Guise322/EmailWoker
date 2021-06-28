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
    public class GMailBoxWorkModel : IEmailBoxWorkModel
    {
        private EmailCredentials emailCredentials;
        public GMailBoxWorkModel(EmailCredentials emailCredentials)
        {
            this.emailCredentials = emailCredentials;
        }
        public async Task<IList<object>> GetUnseenMessagesFromInboxAsync(ImapClient client)
        {
            client.Connect(emailCredentials.MailServer, emailCredentials.Port, emailCredentials.Ssl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(emailCredentials.Login, emailCredentials.Password);
            client.Inbox.Open(FolderAccess.ReadWrite);
            SearchResults unseenMessages = await client.Inbox.SearchAsync(SearchOptions.All,
                SearchQuery.Not(SearchQuery.Seen));
            return (IList<object>) unseenMessages.UniqueIds;
        }
    }
}