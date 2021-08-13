using System.Collections.Generic;

namespace EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages
{
    public interface IHandlerOfProcessedMessage
    {
        (string emailText, string emailSubject) HandleProcessedMessages(List<object> messages);
    }
}