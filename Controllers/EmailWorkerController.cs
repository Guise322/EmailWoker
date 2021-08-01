using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.Infrastructure.Services;
using EmailWorker.Infrastructure.Services.EmailProcessor.Builders;
using MailKit.Net.Imap;

namespace EmailWorker.Controllers
{
    public class EmailWorkerController
    {
        private static string MyEmail { get; } = "guise322@yandex.ru";
        public static async Task ProcessEmailsAsync()
        {
            ImapClient client = new();
            List<EmailCredentials> emailCredentialsList = EmailCredentialsGetter
                .GetEmailCredentials();
            List<IEmailProcessorService> processors = EmailProcessorsFactory
                .CreateEmailProcessorService(emailCredentialsList, client, MyEmail);
            foreach (var processor in processors)
            {
                await processor.ProcessEmailBoxAsync();
            }
        }
    }    
}