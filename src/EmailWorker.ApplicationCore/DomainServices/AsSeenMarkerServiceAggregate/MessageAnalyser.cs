using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using MailKit;

namespace EmailWorker.ApplicationCore.DomainServices.AsSeenMarkerServiceAggregate
{
    public static class MessageAnalyser
    {
        public static bool AnalyseMessages(IList<UniqueId> messages)
        {
            Guard.Against.Null(messages, nameof(messages));
            
            int minNumberOfMessages = 5;

            if(messages.Count < minNumberOfMessages)
            {
                return false;
            }
            
            return true;
        }
    }
}