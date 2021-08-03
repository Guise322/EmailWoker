using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EmailWorker.ApplicationCore.Interfaces;

namespace EmailWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IEmailBoxProcessorService emailBoxProcessorService;
        
        public Worker(ILogger<Worker> logger, IEmailBoxProcessorService emailBoxProcessorService)
        {
            this.logger = logger;
            this.emailBoxProcessorService = emailBoxProcessorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await emailBoxProcessorService.ProcessEmailBoxAsync();
                }
                catch (Exception e)
                {
                    if(e != null)
                        logger.LogError(e.Message);
                    else
                        logger.LogInformation("Worker is over for an error at {time}", DateTimeOffset.Now);
                }
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }   
}
