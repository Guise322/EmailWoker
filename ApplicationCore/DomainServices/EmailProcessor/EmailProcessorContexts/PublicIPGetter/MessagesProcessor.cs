using System.Collections.Generic;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.PublicIPGetter
{
    public class MessagesProcessor
    {
        public static List<object> ProcessMessage(
            List<object> messages,
            string emailAddress)
        {
            
            foreach (var item in messages)
            {
                
            }
            
            return null;
        }
    }
}