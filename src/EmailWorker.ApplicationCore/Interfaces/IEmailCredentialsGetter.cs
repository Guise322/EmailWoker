using System.Collections.Generic;
using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IEmailCredentialsGetter
    {
        List<EmailCredentials> GetEmailCredentials();
    }
}