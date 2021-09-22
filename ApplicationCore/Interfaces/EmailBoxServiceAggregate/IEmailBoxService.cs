using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate
{
    public interface IEmailBoxService
    {
        Task<IList<UniqueId>> AnalyzeMessages(EmailCredentials emailCredentials);
        
        (string emailText, string emailSubject) HandleProcessedMessages(
            IList<UniqueId> messages);

        void SendReportMessageViaEmail(EmailCredentials emailCredentials,
            string myEmail,
            string emailSubject,
            string messageText);
    }
}