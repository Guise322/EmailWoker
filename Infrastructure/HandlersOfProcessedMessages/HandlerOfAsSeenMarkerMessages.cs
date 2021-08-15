using System.Collections.Generic;
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
            int emailsOnRequest = 5;
            
            for (int i = 0; i < emailsOnRequest; i++)
            {
                Client.Inbox.AddFlags(messages[i],
                    MessageFlags.Seen, true);
            }
        
            return (messages.Count.ToString(), "The count of messages marked as seen");
        }
    }
}