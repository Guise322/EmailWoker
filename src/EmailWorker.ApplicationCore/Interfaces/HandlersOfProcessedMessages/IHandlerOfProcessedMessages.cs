using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages
{
    public interface IHandlerOfProcessedMessage
    {
        EmailData HandleProcessedMessages(IList<UniqueId> messages);
    }
}