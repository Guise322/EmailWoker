using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;

namespace EmailWorker.ApplicationCore.DomainServices;

public class EmailInboxServiceBase
{
    public EmailCredentials EmailCredentials { get; set; }
    protected readonly IReportSender ReportSender;
    protected readonly IUnseenMessageIDListGetter GetterOfUnseenMessageIDs;
    protected readonly IClientConnector ClientConnector;
    public EmailInboxServiceBase(
        IReportSender reportSender,
        IUnseenMessageIDListGetter getterOfUnseenMessageIDs,
        IClientConnector clientConnector
    ) => 
    (ReportSender, GetterOfUnseenMessageIDs, ClientConnector) =
    (reportSender, getterOfUnseenMessageIDs, clientConnector);
    
    public Task<IList<UniqueId>> GetUnseenMessageIDsFromEmail()
    {
        ClientConnector.ConnectClient(EmailCredentials);
        return GetterOfUnseenMessageIDs.GetUnseenMessageIDsAsync(EmailCredentials);
    }
}