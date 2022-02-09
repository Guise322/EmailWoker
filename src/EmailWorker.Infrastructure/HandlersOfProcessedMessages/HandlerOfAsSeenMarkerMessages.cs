using Ardalis.GuardClauses;
using System.Collections.Generic;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfAsSeenMarkerMessages : IHandlerOfAsSeenMarkerMessages
    {
        private IMailStore Client { get; }
        public HandlerOfAsSeenMarkerMessages(IMailStore client) => Client = client;
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));

            foreach (var message in messages)
            {
                Client.Inbox.AddFlags(message, MessageFlags.Seen, true);
            }
            
            int maxNumberOfMessages = 1000;

            if (messages.Count < maxNumberOfMessages)
            {
                return (messages.Count.ToString(), "The count of messages marked as seen");    
            }

            return (null, null);
        }
    }
}