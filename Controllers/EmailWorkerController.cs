using System;
using EmailWorker.Models;
using EmailWorker.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;
using EmailWorker.Models.Interfaces;
using MailKit.Search;
using System.Threading.Tasks;

namespace EmailWorker.Controllers
{
    public class EmailWorkerController
    {
        public async static Task ProcessEmailsAsync()
        {
            List<IEmailWorkModel> models = GetEmailModels();
            foreach (var model in models)
            {
                await model.ProcessEmailboxAsync();
            }
        }
        private static List<IEmailWorkModel> GetEmailModels()
        {
            List<IEmailWorkModel> modelsList = new();

            foreach (var item in GetEmailCredentials())
            {
                switch (item.DedicatedWork)
                {
                    case DedicatedWorks.SearchRequest:
                        modelsList.Add(BuildEmailWorkModel<GetPublicIPByEmailModel>(item));
                        break;
                    case DedicatedWorks.MarkAsSeen:
                        modelsList.Add(BuildEmailWorkModel<MarkAsSeen>(item)); 
                        break;
                    default:
                        break;
                }
            }
            return modelsList;
        }
        private static IEmailWorkModel BuildEmailWorkModel<T>(EmailCredentials emailCredentials)
            where T : IEmailWorkModel
        {
            EmailBoxes emailBox = CheckEmailBox(emailCredentials);
            IEmailBoxWorkModel emailBoxWork = GetEmailBoxWorkModel(emailBox, emailCredentials);
            IEmailWorkModel workModel = (T)Activator.CreateInstance(typeof(T), emailCredentials, emailBoxWork);
            return workModel;
        }
        private static List<EmailCredentials> GetEmailCredentials()
        {
            // Create and add a converter which will use the string representation instead of the numeric value.
            JsonStringEnumConverter stringEnumConverter = new();
            JsonSerializerOptions opts = new();
            opts.Converters.Add(stringEnumConverter);
            string jsonString = File.ReadAllText("EmailCredentials.json");
            return JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);
        }
        private static EmailBoxes CheckEmailBox(EmailCredentials emailCredentials) =>
            (emailCredentials.Login.Contains("ya")
                || emailCredentials.Login.Contains("yandex")
            ) ? EmailBoxes.Yandex : EmailBoxes.Google;
        private static IEmailBoxWorkModel GetEmailBoxWorkModel(EmailBoxes emailBoxes,
            EmailCredentials emailCredentials) => emailBoxes switch
        {
            EmailBoxes.Yandex => new YandexMailBoxWorkModel(emailCredentials),
            EmailBoxes.Google => new GMailBoxWorkModel(emailCredentials),
            _ => null,
        };
    }    
}