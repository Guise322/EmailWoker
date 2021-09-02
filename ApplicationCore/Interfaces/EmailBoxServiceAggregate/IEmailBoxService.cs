using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate
{
    public interface IEmailBoxService
    {
        Task<IList<UniqueId>> GetUnseenMessagesAsync(EmailCredentials emailCredentials);
        IList<UniqueId> ProcessMessages(IList<UniqueId> messages);
        (string emailText, string emailSubject) HandleProcessedMessages(IList<UniqueId> messages);
        MimeMessage BuildAnswerMessage(
            EmailCredentials emailCredentials,
            string myEmail,
            string emailSubject,
            string messageText);
        void SendAnswerBySmtp(
            MimeMessage message, EmailCredentials emailCredentials);
    }
}