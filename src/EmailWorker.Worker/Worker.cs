using System;
using System.Threading;
using System.Threading.Tasks;
using EmailWorker.ApplicationCore.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEntryPointService _entryPointService;
        
        public Worker(ILogger<Worker> logger, IEntryPointService emailBoxProcessorService)
        {
            _logger = logger;
            _entryPointService = emailBoxProcessorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _entryPointService.ExecuteAsync();
                }
                catch (Exception e)
                {
                    if(e != null)
                    {
                        _logger.LogError(e.Message);
                    }
                    else
                    {
                        _logger.LogInformation("Worker is over for an error at {time}", DateTimeOffset.Now);
                    }
                }

                TimeSpan workerDelayPeriod = TimeSpan.FromMinutes(5);

                await Task.Delay(workerDelayPeriod, stoppingToken);
            }
        }
    }   
}