using System.Collections.Generic;

namespace EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.AsSeenMarkerAggregate
{
    public class MessagesProcessor
    {
        public static List<object> ProcessMessages(List<object> messages)
        {
            int emailsOnRequest = 5;

            if(messages.Count == 0 || messages.Count < emailsOnRequest)
            {
                return null;
            }

            List<object> processedMessages = new();
            for (int i = 0; i < emailsOnRequest; i++)
            {
                processedMessages.Add(messages[i]);
            }

            return processedMessages;
        }
    }
}