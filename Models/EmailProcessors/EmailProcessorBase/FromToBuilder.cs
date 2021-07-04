using MimeKit;

namespace EmailWorker.Models.EmailProcessors.EmailProcessorBase
{
    public class FromToBuilder
    {
        public static MimeMessage BuildFromTo(IEmailProcessorModel model)
        {
            MimeMessage message = new ();
            message.From.Add(new MailboxAddress("Worker", model.EmailCredentials.Login));
            message.To.Add(new MailboxAddress("Dmitry", model.MyEmail));
            return message;
        }
    }
}