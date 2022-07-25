using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using EmailWorker.Application;
using EmailWorker.Application.Interfaces;

namespace EmailWorker.Infrastructure;

public class EmailCredentialsGetter : IEmailCredentialsGetter
{
    public List<EmailCredentials> GetEmailCredentialsList()
    {
        JsonStringEnumConverter stringEnumConverter = new();
        JsonSerializerOptions opts = new();
        opts.Converters.Add(stringEnumConverter);
        string jsonString = File.ReadAllText("EmailCredentials.json");
        List<EmailCredentials>? emailCredentials = 
            JsonSerializer.Deserialize<List<EmailCredentials>>(jsonString, opts);

        if (emailCredentials is null)
        {
            throw new JsonException("Cannot deserialize EmailCredentials!");
        }

        return emailCredentials;
    }
}