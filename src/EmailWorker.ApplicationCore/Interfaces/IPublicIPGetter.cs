using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces;

public interface IPublicIPGetter
{
    EmailData GetPublicIP(List<UniqueId> messages);
}