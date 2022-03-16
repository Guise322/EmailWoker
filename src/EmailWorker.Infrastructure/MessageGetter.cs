using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailWorker.Infrastructure
{
    public class MessageGetter : IMessageGetter
    {
        private IImapClient Client { get; }
        public MessageGetter(IImapClient client) => Client = client;
        public MimeMessage GetMessage(UniqueId id)
        {
            return Client.Inbox.GetMessage(id);
        }
    }
}