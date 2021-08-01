using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServicesImplementations
{
    public class ProcessedEmailsHandler : IProcessedMessagesHandler
    {
        private ImapClient Client { get; }
        public ProcessedEmailsHandler(ImapClient client)
        {
            Client = client;
        }
        public void HandleProcessedMessages(List<object> messages)
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
        }
    }
}