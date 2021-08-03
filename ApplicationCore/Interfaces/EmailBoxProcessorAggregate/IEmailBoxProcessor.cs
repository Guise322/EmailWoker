using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxProcessorAggregate
{
    public interface IEmailBoxProcessor
    {
        Task<List<object>> GetUnseenMessagesAsync(EmailCredentials emailCredentials);
        List<object> ProcessMessages(List<object> messages);
        (string emailText, string emailSubject) HandleProcessedMessages(List<object> messages);
        MimeMessage BuildAnswerMessage(
            EmailCredentials emailCredentials,
            string myEmail,
            string emailSubject,
            string messageText);
        void SendAnswerBySmtp(
            MimeMessage message, EmailCredentials emailCredentials);
    }
}