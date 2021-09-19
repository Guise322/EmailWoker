using Ardalis.GuardClauses;
using System.Collections.Generic;
using MailKit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public static class MessagesAnalyzer
    {
        public static IList<UniqueId> AnalyzeMessages(ILogger logger, IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));
            
            int minNumberOfMessages = 5;
            int maxNumberOfMessages = 1000;

            if(messages.Count < minNumberOfMessages)
            {
                logger.LogInformation(
                        "The service did not get the needed number of messages.");
                return null;
            }
            
            List<UniqueId> processedMessages = new();

            if (messages.Count < maxNumberOfMessages)
            {
                foreach (var message in messages)
                {
                    processedMessages.Add(message);    
                }

                return processedMessages;
            }

            for (int i = 0; i < maxNumberOfMessages; i++)
            {
                processedMessages.Add(messages[i]);
            }
            
            logger.LogInformation("Getting a chunk of unseen messages succeeds.");
            return processedMessages;
        }
    }
}