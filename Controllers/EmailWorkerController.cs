using System;
using EmailWorker.Models;
using EmailWorker.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;

namespace EmailWorker.Controllers
{
    public class EmailWorkerController
    {
        IEmailModel _model;
        
        public EmailWorkerController()
        {
            _model = new GetPublicIPByEmailModel();
        }

        public void ProcessEmails()
        {
            List<EmailCredentials> credentialsList = GetEmailCredentials();
            _model.SeedEmailCredentials(credentialsList[0]);
            _model.DoProcess();
            //to do: implement the functioning of several models and credentials
        }
        private List<EmailCredentials> GetEmailCredentials()
        {
            // Create and add a converter which will use the string representation instead of the numeric value.
            var stringEnumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
            JsonSerializerOptions opts = new();
            opts.Converters.Add(stringEnumConverter);

            string jsonString = File.ReadAllText("EmailCredentials.json");
            return JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);
        }
    }
}