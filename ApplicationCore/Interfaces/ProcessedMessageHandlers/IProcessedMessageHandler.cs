using System.Collections.Generic;

namespace EmailWorker.ApplicationCore.Interfaces.ProcessedMessageHandlers
{
    public interface IProcessedMessageHandler
    {
        (string emailText, string emailSubject) HandleProcessedMessages(List<object> messages);
    }
}