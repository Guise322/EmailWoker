using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.Shared;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate
{
    public class PublicIPGetterService : IPublicIPGetterService
    {
        private readonly ILogger<PublicIPGetterService> _logger;
        
        private IReportSender ReportSender { get; set; }
        private IMessageGetter MessageGetter { get; set; }
        private IGetterOfUnseenMessages GetterOfUnseenMessages { get; set; }
        private IHandlerOfPublicIPGetterMessages HandlerOfProcessedMessages { get; set; }
        private IClientConnector ClientConnector { get; set; }
        
        private string SearchedEmail { get; } = "guise322@ya.ru";
        public PublicIPGetterService(ILogger<PublicIPGetterService> logger,
            IMessageGetter messageGetter,
            IHandlerOfPublicIPGetterMessages handlerOfProcessedMessages,
            IReportSender reportSender,
            IGetterOfUnseenMessages getterOfUnseenMessages,
            IClientConnector clientConnector) =>

            (_logger, ReportSender, MessageGetter, GetterOfUnseenMessages,
                HandlerOfProcessedMessages, ClientConnector) =
            (logger, reportSender, messageGetter, getterOfUnseenMessages,
                handlerOfProcessedMessages, clientConnector);

        public async Task ProcessEmailInbox(EmailCredentials emailCredentials)
        {
            IList<UniqueId> messages = 
                await MessagesFromEmailGetter.GetMessagesFromEmail(ClientConnector,
                    GetterOfUnseenMessages,
                    emailCredentials);

            //TO DO: extract the below code into an distinct method
            UniqueId searchedMessageID = messages.FirstOrDefault(message => 
            {
                MimeMessage messageFromBox = MessageGetter.GetMessage(message);
                string rawEmailFrom = messageFromBox.From.ToString();
                string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);
                return emailFrom == SearchedEmail;
            });

            if (searchedMessageID == default)
            {
                _logger.LogInformation("The request is not detected.");
                return;       
            }

            _logger.LogInformation("The request is detected.");

            (string emailText, string emailSubject) =
                HandlerOfProcessedMessages.HandleProcessedMessages(messages);

            MimeMessage message = ReportMessageBuilder.BuildReportMessage(emailCredentials,
                SearchedEmail,
                emailSubject,
                emailText);
            
            ReportSender.SendReportViaSmtp(message, emailCredentials);
        }
    }
}