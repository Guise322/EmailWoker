using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using EmailWorker.Application;
using EmailWorker.Application.Exceptions;
using EmailWorker.Application.Interfaces;

namespace EmailWorker.Infrastructure;

public class EmailCredentialsGetter : IEmailCredentialsGetter
{
    public async Task<ReadOnlyCollection<EmailCredentials>?> GetEmailCredentialsCollection(CancellationToken stoppingToken)
    {
        try
        {
            return await GetEmailCredentialsCollectionPrivate(stoppingToken);
        }
        //TO DO: add more exception catches
        catch (JsonException e)
        {
            throw new EmailCredentialsGetterException("Cannot properly read the file with email credentials", e);
        }
        catch (FileNotFoundException e)
        {
            throw new EmailCredentialsGetterException("File not found", e);
        }
    }

    private static async Task<ReadOnlyCollection<EmailCredentials>?>GetEmailCredentialsCollectionPrivate(
        CancellationToken stoppingToken)
    {
        var stringEnumConverter = new JsonStringEnumConverter();
        var opts = new JsonSerializerOptions();

        opts.Converters.Add(stringEnumConverter);
        using FileStream jsonStream = File.Open("EmailCredentials.json", FileMode.Open);
        var emailCredentialsList =
            await JsonSerializer.DeserializeAsync<List<EmailCredentials>>(jsonStream, opts, stoppingToken);

        return emailCredentialsList?.AsReadOnly();
    }
}