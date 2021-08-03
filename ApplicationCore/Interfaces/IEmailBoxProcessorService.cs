using System.Threading.Tasks;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IEmailBoxProcessorService
    {
        Task ProcessEmailBoxAsync();
    }   
}