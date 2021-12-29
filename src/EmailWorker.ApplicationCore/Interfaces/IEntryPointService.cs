using System.Threading.Tasks;

namespace EmailWorker.ApplicationCore.Interfaces
{
    public interface IEntryPointService
    {
        Task ExecuteAsync();
    }   
}