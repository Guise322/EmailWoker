using System;
using EmailWorker.Models;
using EmailWorker.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;
using MailKit.Search;

namespace EmailWorker.Controllers
{
    public class EmailWorkerController
    {
        public EmailWorkerController()
        {

        }
        public void ProcessEmails()
        {
            List<IEmailModel> modelsList = GetEmailModels();
            foreach (var item in modelsList)
            {
                item.ProcessEmailbox();
            }
        }
        private List<IEmailModel> GetEmailModels()
        {
            List<EmailCredentials> emailCredentialsList = GetEmailCredentials();
            List<IEmailModel> modelsList = new();
            foreach (var item in emailCredentialsList)
            {
                switch (item.DedicatedWork)
                {
                    case DedicatedWorks.SearchRequest:
                        modelsList.Add(new GetPublicIPByEmailModel(item));
                        break;
                    case DedicatedWorks.MarkAsSeen:
                        modelsList.Add(new MarkAsSeen(item));
                        break;
                    default:
                        break;
                }
            }
            return modelsList;
        }
    private List<EmailCredentials> GetEmailCredentials()
        {
            // Create and add a converter which will use the string representation instead of the numeric value.
            JsonStringEnumConverter stringEnumConverter = new();
            JsonSerializerOptions opts = new();
            opts.Converters.Add(stringEnumConverter);

            string jsonString = File.ReadAllText("EmailCredentials.json");
            return JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);
        }
    }
}