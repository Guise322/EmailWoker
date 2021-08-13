using System.Collections.Generic;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailProcessor.HandlersOfProcessedMessages
{
    public class HandlerOfAsSeenMarkerMessages : IHandlerOfAsSeenMarkerMessages
    {
        private ImapClient Client { get; }
        public HandlerOfAsSeenMarkerMessages(ImapClient client)
        {
            Client = client;
        }
        public (string emailText, string emailSubject) HandleProcessedMessages(
            List<object> messages)
        {
            int emailsOnRequest = 5;
            if (messages[0] is UniqueId)
            {
                for (int i = 0; i < emailsOnRequest; i++)
                {
                    Client.Inbox.AddFlags((UniqueId)messages[i],
                        MessageFlags.Seen, true);
                }
            }
            else
            {
                for (int i = 0; i < emailsOnRequest; i++)
                {
                    Client.Inbox.AddFlags((int)messages[i],
                        MessageFlags.Seen, true);   
                }
            }

            return (messages.Count.ToString(), "The count of messages marked as seen");
        }
    }
}