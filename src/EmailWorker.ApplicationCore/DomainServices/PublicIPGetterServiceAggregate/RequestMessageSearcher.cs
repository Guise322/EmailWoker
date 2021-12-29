using System.Collections.Generic;
using System.Linq;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate
{
    public class RequestMessageSearcher
    {
        public static UniqueId SearchRequestMessage(IList<UniqueId> messageIDs,
            IMessageGetter messageGetter,
            string searchedEmail) =>
        
            messageIDs.FirstOrDefault(message => 
            {
                MimeMessage messageFromBox = messageGetter.GetMessage(message);
                string rawEmailFrom = messageFromBox.From.ToString();
                string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);
                return emailFrom == searchedEmail;
            });
    }
}