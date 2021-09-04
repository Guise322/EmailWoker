using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages
{
    public class HandlerOfAsSeenMarkerMessages : IHandlerOfAsSeenMarkerMessages
    {
        private readonly ILogger _logger;
        private ImapClient Client { get; }
        public HandlerOfAsSeenMarkerMessages(
            ILogger<HandlerOfAsSeenMarkerMessages> logger,
            ImapClient client)
        {
            _logger = logger;
            Client = client;
        }
        public (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages)
        {
            foreach (var message in messages)
            {
                Client.Inbox.AddFlags(message, MessageFlags.Seen, true);
            }
            
            int maxNumberOfMessages = 1000;

            if (messages.Count < maxNumberOfMessages)
            {
                _logger.LogInformation("All the messages is marked as seen.");

                return (messages.Count.ToString(), "The count of messages marked as seen");    
            }

            return (null, null);
        }
    }
}