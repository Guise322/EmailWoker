using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxProcessorAggregate;
using EmailWorker.Infrastructure;
using MailKit;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices
{
    public class EntryPointService : IEntryPointService
    {
        private readonly string myEmail  = "guise322@yandex.ru";
        private readonly IServiceScopeFactory serviceScopeFactory;
        public EntryPointService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task ExecuteAsync()
        {
            List<EmailCredentials> emailCredentialsList = EmailCredentialsGetter
                .GetEmailCredentials();
            using var serviceScope = serviceScopeFactory.CreateScope();
            foreach (var emailCredentials in emailCredentialsList)
            {
                var emailBoxProcessor = emailCredentials.DedicatedWork switch
                {
                    DedicatedWorkType.MarkAsSeen => serviceScope.ServiceProvider
                        .GetRequiredService<IAsSeenMarkerProcessor>(),
                    DedicatedWorkType.SearchRequest => serviceScope.ServiceProvider
                        .GetRequiredService<IPublicIPGetterProcessor>(),
                    _ => null
                };

                IList<UniqueId> unseenMessages = await emailBoxProcessor.GetUnseenMessagesAsync(emailCredentials);
                IList<UniqueId> processedMessages = emailBoxProcessor.ProcessMessages(unseenMessages);
                if (processedMessages != null)
                {
                    (string emailText, string emailSubject) = 
                        emailBoxProcessor.HandleProcessedMessages(processedMessages);

                    MimeMessage answerMessage = emailBoxProcessor.BuildAnswerMessage(
                                                    emailCredentials,
                                                    myEmail,
                                                    emailSubject,
                                                    emailText);
                    emailBoxProcessor.SendAnswerBySmtp(answerMessage, emailCredentials);
                }
            }
        }
    }    
}