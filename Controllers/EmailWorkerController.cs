using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.Models.EmailProcessors;
using EmailWorker.Models.EmailProcessors.EmailProcessorBuilder;
using EmailWorker.Shared;

namespace EmailWorker.Controllers
{
    public class EmailWorkerController
    {
        public async static Task ProcessEmailsAsync()
        {
            List<IEmailProcessor> processors = new();
            List<EmailCredentials> emailCredentialsList = EmailCredentialsGetter.GetEmailCredentials();
            foreach (var item in emailCredentialsList)
            {
                processors.Add(Builder.BuildEmailProcessor(item));
            }
            await ProcessorActivator.ActivateProcessors(processors);
        }
    }    
}