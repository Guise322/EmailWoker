using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailWorker.Infrastructure.EmailServicesImplementations
{
    public class MessageGetter : IMessageGetter
    {
        private ImapClient Client { get; }
        public MessageGetter(ImapClient client)
        {
            Client = client;
        }
        public MimeMessage GetMessage(UniqueId id)
        {
            return Client.Inbox.GetMessage(id);
        }
        public MimeMessage GetMessage(int index)
        {
            return Client.Inbox.GetMessage(index);
        }
    }
}