using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWorker.Models.EmailProcessors;

namespace EmailWorker.Controllers
{
    class ProcessorActivator
    {
        public static async Task ActivateProcessors(List<IEmailProcessor> emailProcessors)
        {
            foreach (var processor in emailProcessors)
            {
                await processor.ProcessEmailBoxAsync();
            }
        }
    }
}