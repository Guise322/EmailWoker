using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;
using EmailWorker.Infrastructure.EmailServices.UnseenMessagesGetter;
using EmailWorker.Infrastructure.EmailServicesImplementations;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices.Builders
{
    public class EmailServicesBaseBuilder
    {
        public static EmailServicesBase Build(ImapClient client)
        {
            return new EmailServicesBase()
            {
                AnswerSender = new AnswerSender(),
                ProcessedMessagesHandler = new ProcessedEmailsHandler(client),
                UnseenMessagesGetter = new InboxGetter(client),
                ClientConnector = new ClientConnector(client)
            };
        }
    }
}