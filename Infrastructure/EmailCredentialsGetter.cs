using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using EmailWorker.ApplicationCore.Entities;

namespace EmailWorker.Infrastructure.EmailProcessor
{
    public class EmailCredentialsGetter
    {
        public static List<EmailCredentials> GetEmailCredentials()
        {
            JsonStringEnumConverter stringEnumConverter = new();
            JsonSerializerOptions opts = new();
            opts.Converters.Add(stringEnumConverter);
            string jsonString = File.ReadAllText("EmailCredentials.json");
            return JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);
        }
    }
}