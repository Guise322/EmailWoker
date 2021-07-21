using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IProcessedMessagesHandler
    {
        void HandleProcessedMessages(List<object> messages);
    }
}