using Ardalis.GuardClauses;
using System.Collections.Generic;
using MailKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerAggregate
{
    public class MessagesProcessor
    {
        public static IList<UniqueId> ProcessMessages(IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));
            
            int minimumAmount = 5;
            int maximumAmount = 1000;

            if(messages.Count < minimumAmount)
            {
                return null;
            }
            
            List<UniqueId> processedMessages = new();

            if (messages.Count < maximumAmount)
            {
                foreach (var message in messages)
                {
                    processedMessages.Add(message);    
                }

                return processedMessages;
            }

            for (int i = 0; i < maximumAmount; i++)
            {
                processedMessages.Add(messages[i]);
            }

            return processedMessages;
        }
    }
}