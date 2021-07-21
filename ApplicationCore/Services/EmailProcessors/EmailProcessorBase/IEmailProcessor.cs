using System.Threading.Tasks;

namespace EmailWorker.ApplicationCore.Services.EmailProcessors.EmailProcessorBase
{
    public interface IEmailProcessor
    {
        Task ProcessEmailBoxAsync();
    }   
}