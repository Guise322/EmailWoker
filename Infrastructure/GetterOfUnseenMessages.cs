//Some of the below lines of code is got from the Github issue:
//https://github.com/jstedfast/MailKit/issues/266

using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure
{

    public class GetterOfUnseenMessages : IGetterOfUnseenMessages
    {
        private readonly ILogger<GetterOfUnseenMessages> _logger;
        private ImapClient Client { get; }
        public GetterOfUnseenMessages(ILogger<GetterOfUnseenMessages> logger, ImapClient client) =>
            (_logger, Client) = (logger, client);
        public async Task<IList<UniqueId>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            Client.Inbox.Open(FolderAccess.ReadWrite);
            
            IList<UniqueId> unseenMessages = await Client.Inbox.SearchAsync(SearchQuery.NotSeen);

            _logger.LogInformation(
                $"The unseen messages is got from the {emailCredentials.Login} inbox.");

            return unseenMessages;
        }
    }
}