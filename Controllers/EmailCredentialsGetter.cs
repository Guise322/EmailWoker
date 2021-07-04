using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using EmailWorker.Shared;

namespace EmailWorker.Controllers
{
    public class EmailCredentialsGetter
    {
        public static List<EmailCredentials> GetEmailCredentials()
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