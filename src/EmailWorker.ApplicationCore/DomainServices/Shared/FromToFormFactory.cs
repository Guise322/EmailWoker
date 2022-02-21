using EmailWorker.ApplicationCore.Entities;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.Shared
{
    public class FromToFormFactory
    {
        public static MimeMessage CreateFromToForm(EmailCredentials emailCredentials, string emailAddress)
        {
            MimeMessage message = new ();
            message.From.Add(new MailboxAddress("Worker", emailCredentials.Login));
            message.To.Add(new MailboxAddress("Dmitry", emailAddress));
            return message;
        }
    }
}