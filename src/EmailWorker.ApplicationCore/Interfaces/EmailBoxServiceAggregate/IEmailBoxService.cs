using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.ApplicationCore.Interfaces.Services.EmailBoxServiceAggregate
{
    public interface IEmailBoxService
    {
        EmailCredentials EmailCredentials { get; set; }
        Task<ServiceStatus> ProcessEmailInbox();
    }
}