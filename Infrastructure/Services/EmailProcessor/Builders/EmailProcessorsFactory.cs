using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using MailKit.Net.Imap;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.DomainServices.EmailProcessor;

namespace EmailWorker.Infrastructure.Services.EmailProcessor.Builders
{
    public class EmailProcessorsFactory
    {

        public static List<IEmailProcessorService> CreateEmailProcessorService(
            List<EmailCredentials> emailCredentialsList,
            ImapClient client,
            string myEmail
        )
        {
            List<IEmailProcessorService> processors = new();

            foreach (var emailCredentials in emailCredentialsList)
            {
                processors.Add(emailCredentials.DedicatedWork switch
                    {
                        DedicatedWorkType.MarkAsSeen =>
                            new EmailProcessorService(emailCredentials, ServiceContextBuilders
                                .BuildMarkAsSeenContext(client), myEmail),
                        DedicatedWorkType.SearchRequest =>
                            new EmailProcessorService(emailCredentials, ServiceContextBuilders
                            .BuildPublicIPGetterContext(client, myEmail), myEmail),
                        _ => null
                    });
            }

            return processors;
        }
    }
}