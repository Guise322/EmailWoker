using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate
{
    public interface IEmailBoxService
    {
        Task ProcessEmailInbox(EmailCredentials emailCredentials);
    }
}