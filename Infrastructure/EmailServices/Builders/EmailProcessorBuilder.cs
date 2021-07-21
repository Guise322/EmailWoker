using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;
using EmailWorker.ApplicationCore.Services.EmailProcessors.PublicIPGetterProcessor;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices.Builders
{
    public class EmailProcessorBuilder
    {
        public static IEmailProcessor BuildEmailProcessor(EmailCredentials emailCredentials, 
            ImapClient client)
        {
            return new EmailProcessor(EmailEntityBuilder.BuildEmailEntity(emailCredentials),
                EmailServicesBaseBuilder.Build(client));
        }
        public static IEmailProcessor BuildPublicIPGetterProcessor(EmailCredentials emailCredentials, 
            ImapClient client)
        {
            return new PublicIPGetterProcessor(EmailEntityBuilder.BuildEmailEntity(emailCredentials),
                PublicIPGetterServicesBuilder.Build(client));
        }
    }
}