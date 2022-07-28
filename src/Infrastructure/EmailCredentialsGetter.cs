using System.Text.Json;
using System.Text.Json.Serialization;
using EmailWorker.Application;
using EmailWorker.Application.Interfaces;

namespace EmailWorker.Infrastructure;

public class EmailCredentialsGetter : IEmailCredentialsGetter
{
    public List<EmailCredentials>? GetEmailCredentialsList()
    {
        var stringEnumConverter = new JsonStringEnumConverter();
        var opts = new JsonSerializerOptions();

        opts.Converters.Add(stringEnumConverter);
        string jsonString = File.ReadAllText("EmailCredentials.json");
        var emailCredentialsList = 
            JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);

        return emailCredentialsList;
    }
}