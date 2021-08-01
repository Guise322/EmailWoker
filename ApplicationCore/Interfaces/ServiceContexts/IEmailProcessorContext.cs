using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces.ServiceContexts
{
    public interface IEmailProcessorServiceContext
    {
        Task<List<object>> GetUnseenMessagesAsync(EmailCredentials emailCredentials);
        List<object> ProcessMessages(List<object> messages);
        void HandleProcessedMessages(List<object> messages);
        MimeMessage BuildAnswerMessage(List<object> messages,
            MimeMessage messageWithFromTo);
        void SendAnswerBySmtp(MimeMessage message,
            EmailCredentials emailCredentials);
    }
}