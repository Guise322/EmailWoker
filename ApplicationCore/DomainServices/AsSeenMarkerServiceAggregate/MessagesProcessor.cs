using Ardalis.GuardClauses;
using System.Collections.Generic;
using MailKit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public static class MessagesProcessor
    {
        public static IList<UniqueId> ProcessMessages(ILogger logger, IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));
            
            int minimumAmount = 5;
            int maximumAmount = 1000;

            if(messages.Count < minimumAmount)
            {
                logger.LogInformation(
                        "The service did not get the needed number of messages.");
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
            
            logger.LogInformation("Getting of unseen messages succeeds.");
            return processedMessages;
        }
    }
}