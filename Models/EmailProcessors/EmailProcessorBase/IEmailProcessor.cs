using System.Threading.Tasks;

namespace EmailWorker.Models.EmailProcessors
{
    public interface IEmailProcessor
    {
        Task ProcessEmailBoxAsync();
    }   
}