using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;

namespace EmailWorker.ApplicationCore.DomainServices;

public class EmailInboxServiceBase
{
    public EmailCredentials EmailCredentials { get; set; }
    protected readonly IReportSender _reportSender;
    protected readonly IUnseenMessageIDListGetter _unseenMessageIDListGetter;
    protected readonly IClientConnector _clientConnector;
    public EmailInboxServiceBase(
        IReportSender reportSender,
        IUnseenMessageIDListGetter unseenMessageIDListGetter,
        IClientConnector clientConnector
    ) => 
    (_reportSender, _unseenMessageIDListGetter, _clientConnector) =
    (reportSender, unseenMessageIDListGetter, clientConnector);
    
    protected Task<IList<UniqueId>> GetUnseenMessageIDsFromEmail()
    {
        Guard.Against.Null(EmailCredentials, nameof(EmailCredentials));
        _clientConnector.ConnectClient(EmailCredentials);
        return _unseenMessageIDListGetter.GetUnseenMessageIDsAsync(EmailCredentials);
    }
}