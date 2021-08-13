using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailProcessor.GetterOfUnseenMessages
{
    public class InboxGetter : IGetterOfUnseenMessages
    {
        private ImapClient Client { get; }
        public InboxGetter(ImapClient client)
        {
            Client = client;
        }
        public async Task<List<object>> GetUnseenMessagesAsync(EmailCredentials emailCredentials)
        {
            return await GetterBuilder.BuildGetter(emailCredentials, Client)
                .GetUnseenMessagesAsync();
        }
    }
}