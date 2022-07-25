using System;
using System.Threading;
using System.Threading.Tasks;
using EmailWorker.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace EmailWorker.Worker
{
    public class Worker : BackgroundService
    {
        private readonly TimeSpan WorkerDelayPeriod = TimeSpan.FromMinutes(5);
        private readonly IEmailInboxServiceCommand _entryPointService;
        
        public Worker(IEmailInboxServiceCommand emailBoxProcessorService)
        {
            _entryPointService = emailBoxProcessorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _entryPointService.ExecuteAsync();

                await Task.Delay(WorkerDelayPeriod, stoppingToken);
            }
        }

        // TO DO: make the StopAsync method to gracefully stop the background service;
        //          implement the IDisposable interface in all the appropriate classes
    }   
}