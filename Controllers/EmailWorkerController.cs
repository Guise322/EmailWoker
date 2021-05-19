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

        public void PublicIPProcess()
        {
            // Create and add a converter which will use the string representation instead of the numeric value.
            var stringEnumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
            JsonSerializerOptions opts = new JsonSerializerOptions();
            opts.Converters.Add(stringEnumConverter);

            string jsonString = File.ReadAllText("EmailCredentials.json");
            List<EmailCredentials> credentialsList = JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);
            
            //To do: implement handling of a list of emails.

            /*_model.GetEmailCredentials(credentials);

            bool requestIsGot = _model.ProcessResults(_model.GetUnseenMessagesFromInbox());

            if (requestIsGot) _model.SendAnswerBySmtp();*/
        }
    }
}