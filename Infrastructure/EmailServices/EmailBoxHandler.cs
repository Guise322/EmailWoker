using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;
using EmailWorker.Infrastructure.EmailServices.Builders;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices
{
    public class EmailBoxHandler
    {
        public static async Task HandleEmailBoxesAync(ImapClient client)
        {
            List<EmailCredentials> emailCredentialsList = EmailCredentialsGetter.GetEmailCredentials();
            List<IEmailProcessor> processors = EmailProcessorsFactory.Build(emailCredentialsList, client);
            foreach (var processor in processors)
            {
                await processor.ProcessEmailBoxAsync();
            }
        }
    }
}