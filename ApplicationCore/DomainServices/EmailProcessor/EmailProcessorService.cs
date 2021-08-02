using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.ServiceContexts;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.EmailProcessor
{
    public class EmailProcessorService : IEmailProcessorService
    {
        public string MyEmail { get; }
        protected EmailCredentials EmailCredentials { get; set; }
        protected IEmailProcessorServiceContext ServiceContext { get; }
        public EmailProcessorService(
            EmailCredentials emailCredentials,
            IEmailProcessorServiceContext serviceContext,
            string myEmail
        )
        {
            EmailCredentials = emailCredentials;
            ServiceContext = serviceContext;
            MyEmail = myEmail;
        }
        public async Task ProcessEmailBoxAsync()
        {   
            List<object> unseenMessages = await ServiceContext.GetUnseenMessagesAsync(EmailCredentials);
            List<object> processedMessages = ServiceContext.ProcessMessages(unseenMessages);
            if(processedMessages != null)
            {
                ServiceContext.HandleProcessedMessages(unseenMessages);

                MimeMessage messageWithFromTo = FromToBuilder.BuildFromTo(EmailCredentials, MyEmail);
                MimeMessage answerMessage = ServiceContext.BuildAnswerMessage(unseenMessages,
                    messageWithFromTo);
                ServiceContext.SendAnswerBySmtp(answerMessage, EmailCredentials);
            }
        }
    }
}