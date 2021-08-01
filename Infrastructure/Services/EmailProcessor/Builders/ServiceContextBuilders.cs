using EmailWorker.Infrastructure.Services.UnseenMessagesGetter;
using EmailWorker.Infrastructure.EmailServicesImplementations;
using MailKit.Net.Imap;
using EmailWorker.ApplicationCore.Interfaces.ServiceContexts;
using EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.MarkAsSeen;
using EmailWorker.ApplicationCore.DomainServices.EmailProcessor.EmailProcessorContexts.PublicIPGetter;

namespace EmailWorker.Infrastructure.Services.EmailProcessor.Builders
{
    public class ServiceContextBuilders
    {
        public static IMarkAsSeenContext BuildMarkAsSeenContext(ImapClient client)
        {
            return new MarkAsSeenContext
            {
                AnswerSender = new AnswerSender(),
                ProcessedMessagesHandler = new ProcessedEmailsHandler(client),
                UnseenMessagesGetter = new InboxGetter(client),
                ClientConnector = new ClientConnector(client)
            };
        }
        public static IPublicIPGetterContext BuildPublicIPGetterContext(
            ImapClient client, string searchedEmail)
        {
            IMarkAsSeenContext markAsSeenContext = BuildMarkAsSeenContext(client);
            return new PublicIPGetterContext(searchedEmail)
            {
                AnswerSender = markAsSeenContext.AnswerSender,
                ProcessedMessagesHandler = markAsSeenContext.ProcessedMessagesHandler,
                UnseenMessagesGetter = markAsSeenContext.UnseenMessagesGetter,

                MessageGetter = new MessageGetter(client),
                IPGetter = new PublicIPGetter()
            };
        }
    }
}