using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices.UnseenMessagesGetter
{
    public class InboxGetter : IUnseenMessagesGetter
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