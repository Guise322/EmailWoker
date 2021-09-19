using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using EmailWorker.Infrastructure;
using MailKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices
{
    public class EntryPointService : IEntryPointService
    {
        private readonly string myEmail  = "guise322@yandex.ru";
        private readonly ILogger<EntryPointService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly EmailCredentialsGetter _emailCredentialsGetter; 
        public EntryPointService(
            ILogger<EntryPointService> logger,
            IServiceScopeFactory serviceScopeFactory,
            EmailCredentialsGetter emailCredentialsGetter) =>
        (_logger, _serviceScopeFactory, _emailCredentialsGetter) = 
        (logger, serviceScopeFactory, emailCredentialsGetter);
        public async Task ExecuteAsync()
        {
            _logger.LogInformation($"Start execution at {DateTimeOffset.Now}");

            List<EmailCredentials> emailCredentialsList = _emailCredentialsGetter
                .GetEmailCredentials();
                
            using var serviceScope = _serviceScopeFactory.CreateScope();
            foreach (var emailCredentials in emailCredentialsList)
            {
                var emailBoxProcessor = emailCredentials.DedicatedWork switch
                {
                    DedicatedWorkType.MarkAsSeen => serviceScope.ServiceProvider
                        .GetRequiredService<IAsSeenMarkerService>(),
                    DedicatedWorkType.SearchRequest => serviceScope.ServiceProvider
                        .GetRequiredService<IPublicIPGetterService>(),
                    _ => null
                };

                IList<UniqueId> unseenMessages = await emailBoxProcessor.GetUnseenMessagesAsync(emailCredentials);
                IList<UniqueId> processedMessages = emailBoxProcessor.AnalyzeMessages(unseenMessages);
                if (processedMessages != null)
                {
                    (string emailText, string emailSubject) = 
                        emailBoxProcessor.HandleProcessedMessages(processedMessages);

                    if(emailText != null)
                    {
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
}