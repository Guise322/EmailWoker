using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfAsSeenMarkerMessages : IHandlerOfAsSeenMarkerMessages
    {
        private ImapClient Client { get; }
        public HandlerOfAsSeenMarkerMessages(ImapClient client)
        {
            Client = client;
        }
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            foreach (var message in messages)
            {
                Client.Inbox.AddFlags(message, MessageFlags.Seen, true);
            }
        
            return (messages.Count.ToString(), "The count of messages marked as seen");
        }
    }
}