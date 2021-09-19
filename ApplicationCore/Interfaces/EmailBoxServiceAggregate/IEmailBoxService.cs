using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate
{
    public interface IEmailBoxService
    {
        IList<UniqueId> AnalyzeMessages(IList<UniqueId> messages);
        (string emailText, string emailSubject) HandleProcessedMessages(IList<UniqueId> messages);
    }
}