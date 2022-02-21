using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.DomainServices.Shared;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate;
using MailKit;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;

public class PublicIPGetterService : IPublicIPGetterService
{
    public EmailCredentials EmailCredentials { get; set; }
    private IReportSender ReportSender { get; set; }
    private IMessageGetter MessageGetter { get; set; }
    private IGetterOfUnseenMessageIDs GetterOfUnseenMessages { get; set; }
    private IHandlerOfPublicIPGetterMessages HandlerOfProcessedMessages { get; set; }
    private IClientConnector ClientConnector { get; set; }
    
    private string SearchedEmail { get; } = "guise322@ya.ru";
    public PublicIPGetterService(
        IMessageGetter messageGetter,
        IHandlerOfPublicIPGetterMessages handlerOfProcessedMessages,
        IReportSender reportSender,
        IGetterOfUnseenMessageIDs getterOfUnseenMessages,
        IClientConnector clientConnector) =>

        (ReportSender, MessageGetter, GetterOfUnseenMessages,
            HandlerOfProcessedMessages, ClientConnector) =
        (reportSender, messageGetter, getterOfUnseenMessages,
            handlerOfProcessedMessages, clientConnector);

    public async Task<ServiceStatus> ProcessEmailInbox()
    {
        IList<UniqueId> messageIDs = 
            await MessageIDsFromEmailGetter.GetMessageIDsFromEmail(
                ClientConnector,
                GetterOfUnseenMessages,
                EmailCredentials);

        UniqueId searchedMessageID = RequestMessageSearcher
            .SearchRequestMessage(messageIDs, MessageGetter, SearchedEmail);

        //TO DO: inspect the below statement
        if (searchedMessageID == default)
        {
            return new ServiceStatus() { ServiceWorkMessage = "The request is not detected." };
        }
        
        ServiceStatus currentStatus = new () { ServiceWorkMessage = "The request is detected." };

        try
        {
            EmailData emailData = HandlerOfProcessedMessages.HandleProcessedMessages(messageIDs);

            MimeMessage message = ReportMessageFactory.CreateReportMessage(
                EmailCredentials,
                SearchedEmail,
                emailData);
            
            ReportSender.SendReportViaSmtp(message, EmailCredentials);

            currentStatus.ServiceWorkMessage += " The current ip address is sent.";
        }
        catch (System.Exception)
        {
            currentStatus.ServiceWorkMessage += " The current ip address is not sent.";
        }

        return currentStatus;
    }
}