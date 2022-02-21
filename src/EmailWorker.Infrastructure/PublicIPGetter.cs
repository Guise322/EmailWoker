//Some of the below lines of code is got from the site:
//https://qawithexperts.com/article/c-sharp/get-ip-address-using-c-local-and-public-ip-example/374

using System.Collections.Generic;
using System.Net.Http;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces;
using MailKit;
using MailKit.Net.Imap;

namespace EmailWorker.Infrastructure;

public class PublicIPGetter : SeenFlagAdder, IPublicIPGetter
{
    private readonly string ipRequestAddress = "http://checkip.dyndns.org/";
    
    private readonly IHttpClientFactory _httpClientFactory;
    public PublicIPGetter(IHttpClientFactory httpClientFactory, ImapClient client) :
    base (client) =>
        _httpClientFactory = httpClientFactory;
    public EmailData GetPublicIP(List<UniqueId> messages)
    {
        AddSeenFlag(messages);

        string httpClientName = "checkip";

        //TO DO: add checking if the request was successful

        HttpClient httpClient = _httpClientFactory.CreateClient(httpClientName);

        HttpRequestMessage request = new(HttpMethod.Get,ipRequestAddress);

        HttpResponseMessage response = httpClient.Send(request);
        string responseString = response.Content.ReadAsStringAsync().Result;

        string address = 
            AddressFromResponseExtractor.ExtractAddressFromResponse(responseString);
        
        
        //_logger.LogInformation("Getting of the current IP succeeds.");

        return new EmailData()
        {
            EmailSubject = "Ip By Email Service",
            EmailText = address
        };
    }
}