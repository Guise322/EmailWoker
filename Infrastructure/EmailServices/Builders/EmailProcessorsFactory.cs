using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Enums;
using EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase;
using EmailWorker.ApplicationCore.Services.EmailProcessors.PublicIPGetterProcessor;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure.EmailServices.Builders
{
    public class EmailProcessorsFactory
    {
        public static List<IEmailProcessor> Build(List<EmailCredentials> emailCredentialsList,
            ImapClient client)
        {
            List<IEmailProcessor> processors = new();

            foreach (var emailCredentials in emailCredentialsList)
            {
                processors.Add(emailCredentials.DedicatedWork switch
                    {
                        DedicatedWorkType.MarkAsSeen =>
                            EmailProcessorBuilder.BuildEmailProcessor(emailCredentials, client),
                        DedicatedWorkType.SearchRequest =>
                            EmailProcessorBuilder.BuildPublicIPGetterProcessor(emailCredentials, client),
                        _ => null
                    });
            }

            return processors;
        }
    }
}