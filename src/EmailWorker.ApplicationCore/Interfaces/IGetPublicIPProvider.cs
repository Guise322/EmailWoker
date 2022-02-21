using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;
using MailKit;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IGetPublicIPProvider
    {
        EmailData GetPublicIPProvider(List<UniqueId> messages);
    }
}