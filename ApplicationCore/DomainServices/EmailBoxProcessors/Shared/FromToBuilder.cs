using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailBoxProcessors.Shared
{
    public class FromToBuilder
    {
        public static MimeMessage BuildFromTo(EmailCredentials emailCredentials, string emailAddress)
        {
            MimeMessage message = new ();
            message.From.Add(new MailboxAddress("Worker", emailCredentials.Login));
            message.To.Add(new MailboxAddress("Dmitry", emailAddress));
            return message;
        }
    }
}