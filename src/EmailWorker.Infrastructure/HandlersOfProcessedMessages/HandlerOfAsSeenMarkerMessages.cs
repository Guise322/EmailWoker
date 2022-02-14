using Ardalis.GuardClauses;
using System.Collections.Generic;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using MailKit.Net.Imap;
using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfAsSeenMarkerMessages : IHandlerOfAsSeenMarkerMessages
    {
        private IMailStore Client { get; }
        public HandlerOfAsSeenMarkerMessages(IMailStore client) => Client = client;
        public EmailData HandleProcessedMessages(IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));

            foreach (var message in messages)
            {
                Client.Inbox.AddFlags(message, MessageFlags.Seen, true);
            }
            
            int maxNumberOfMessages = 1000;

            if (messages.Count < maxNumberOfMessages)
            {
                return new EmailData()
                {
                    EmailSubject = "The count of messages marked as seen",
                    EmailText = messages.Count.ToString()
                };    
            }

            return null;
        }
    }
}