using System.Collections.Generic;
using MailKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.AsSeenMarkerAggregate
{
    public class MessagesProcessor
    {
        public static IList<UniqueId> ProcessMessages(IList<UniqueId> messages)
        {
            int emailsOnRequest = 5;

            if(messages.Count == 0 || messages.Count < emailsOnRequest)
            {
                return null;
            }

            List<UniqueId> processedMessages = new();
            for (int i = 0; i < emailsOnRequest; i++)
            {
                processedMessages.Add(messages[i]);
            }

            return processedMessages;
        }
    }
}