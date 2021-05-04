using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IpByEmail.MailRepository;

namespace IpByEmail
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //dimsonartex@gmail.com
        private MailRepositoryClass _mailRepositoryClass = new MailRepositoryClass("imap.gmail.com", 993, 
            true, "dimsonartex@gmail.com", "17890714");

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IEnumerable<string> unreadMails;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    unreadMails = _mailRepositoryClass.GetUnreadMails();
                }
                catch (System.Exception e)
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
