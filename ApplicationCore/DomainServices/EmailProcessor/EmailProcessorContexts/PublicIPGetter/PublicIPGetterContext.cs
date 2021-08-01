using System.Collections.Generic;
using EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.MarkAsSeen;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.ServiceContexts;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.PublicIPGetter
{
    public class PublicIPGetterContext : MarkAsSeenContext, IPublicIPGetterContext
    {
        private string SearchedEmail { get; }
        public PublicIPGetterContext(string searchedEmail)
        {
            SearchedEmail = searchedEmail;
        }
        public IMessageGetter MessageGetter { get; set; }
        public IPublicIPGetter IPGetter { get; set; }
        public override List<object> ProcessMessages(List<object> messages) =>
            MessagesProcessor.ProcessMessages(messages, SearchedEmail, MessageGetter);
        public override MimeMessage BuildAnswerMessage(List<object> messages,
            MimeMessage messageWithFromTo) =>
            AnswerMessageBuilder.BuildAnswerMessage(messageWithFromTo, IPGetter);
    }
}