using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using MailKit;
using Microsoft.Extensions.Logging;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public static class AnalyzerOfMessages
    {
        public static IList<UniqueId> AnalyzeMessages(IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));
            
            int minNumberOfMessages = 5;
            int maxNumberOfMessages = 1000;

            if(messages.Count < minNumberOfMessages)
            {
                throw new ArgumentException("The given number of messages is too small.");
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
            
            return processedMessages;
        }
    }
}