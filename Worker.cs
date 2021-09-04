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
        private readonly ILogger<Worker> _logger;
        private readonly IEntryPointService entryPointService;
        
        public Worker(ILogger<Worker> logger, IEntryPointService emailBoxProcessorService)
        {
            _logger = logger;
            this.entryPointService = emailBoxProcessorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await entryPointService.ExecuteAsync();
                }
                catch (Exception e)
                {
                    if(e != null)
                        _logger.LogError(e.Message);
                    else
                        _logger.LogInformation("Worker is over for an error at {time}", DateTimeOffset.Now);
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }   
}
