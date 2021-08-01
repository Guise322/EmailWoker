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
        public MimeMessage GetMessage(object id)
        {
            return (id is UniqueId uniqueID) ? Client.Inbox.GetMessage(uniqueID) : 
                Client.Inbox.GetMessage((int)id);
        }
    }
}