using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;
using EmailWorker.ApplicationCore.Services.EmailProcessors.PublicIPGetterProcessor;
using EmailWorker.Infrastructure.EmailServicesImplementations;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices.Builders
{
    public class PublicIPGetterServicesBuilder
    {
        public static PublicIPGetterServices Build(ImapClient client)
        {
            EmailServicesBase servicesBase = EmailServicesBaseBuilder.Build(client);
            return new PublicIPGetterServices()
            {
                AnswerSender = servicesBase.AnswerSender,
                ProcessedMessagesHandler = servicesBase.ProcessedMessagesHandler,
                UnseenMessagesGetter = servicesBase.UnseenMessagesGetter,

                MessageGetter = new MessageGetter(client),
                IPGetter = new PublicIPGetter()
            };
        }
    }
}