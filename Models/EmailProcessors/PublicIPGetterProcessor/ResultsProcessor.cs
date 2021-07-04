using System.Collections.Generic;
using EmailWorker.Models;
using EmailWorker.Models.EmailProcessors.EmailProcessorBase;
using MailKit;
using MimeKit;

namespace EmailWorker.Controllers.EmailProcessors.PublicIPGetterProcessor
{
    public class ResultsProcessor
    {
        public static bool ProcessResults(IEmailProcessorModel model)
        {
            bool isUniqueId = model.UnseenMessages is IList<UniqueId>;
            foreach (var item in model.UnseenMessages)
            {
                MimeMessage message = isUniqueId ? model.Client.Inbox.GetMessage((UniqueId)item) :
                    model.Client.Inbox.GetMessage((int)item);
                string rawEmailFrom = message.From.ToString();

                string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);

                if (emailFrom == model.MyEmail)
                {
                    return true;
                }
            }
            model.Client.Disconnect(true);
            return false;
        }
    }
}