using System.Threading.Tasks;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IEmailProcessorService
    {
        Task ProcessEmailBoxAsync();
    }   
}