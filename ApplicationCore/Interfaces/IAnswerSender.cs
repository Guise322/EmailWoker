
using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IAnswerSender
    {
        void SendAnswerBySmtp(MimeMessage message, EmailCredentials emailCredentials);
    }
}