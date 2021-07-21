using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.PublicIPGetterProcessor
{
    public class MessagesProcessor
    {
        public static List<object> ProcessMessages(List<object> messages, string emailAddress,
            IMessageGetter messageGetter)
        {
            bool isUniqueId = messages is IList<UniqueId>;
            foreach (var item in messages)
            {
                MimeMessage message = isUniqueId ? messageGetter.GetMessage((UniqueId)item) :
                    messageGetter.GetMessage((int)item);
                string rawEmailFrom = message.From.ToString();

                string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);

                if (emailFrom == emailAddress)
                {
                    return new List<object>(1) { item };
                }
            }
            
            return null;
        }
    }
}