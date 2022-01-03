using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailWorker.Infrastructure
{
    public class EmailCredentialsGetter : IEmailCredentialsGetter
    {
        private readonly ILogger<EmailCredentialsGetter> _logger;
        public EmailCredentialsGetter(ILogger<EmailCredentialsGetter> logger) =>
            _logger = logger;
        public List<EmailCredentials> GetEmailCredentials()
        {
            JsonStringEnumConverter stringEnumConverter = new();
            JsonSerializerOptions opts = new();
            opts.Converters.Add(stringEnumConverter);
            string jsonString = File.ReadAllText("EmailCredentials.json");
            List<EmailCredentials> emailCredentials = 
                JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);
            _logger.LogInformation("EmailCredentials is got.");
            return emailCredentials;
        }
    }
}