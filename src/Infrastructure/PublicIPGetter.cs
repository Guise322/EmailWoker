//Some of the below lines of code is got from the site:
//https://qawithexperts.com/article/c-sharp/get-ip-address-using-c-local-and-public-ip-example/374

using EmailWorker.Application;
using EmailWorker.Application.Interfaces;
using MailKit;

namespace EmailWorker.Infrastructure;

public class PublicIPGetter : IPublicIPGetter
{
    private const string _httpClientName = "checkip";
    private const string _ipRequestAddress = "http://checkip.dyndns.org/";
    private readonly IHttpClientFactory _httpClientFactory;
    
    public PublicIPGetter(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public EmailData GetPublicIP()
    {
        //TO DO: add checking if the request was successful

        var httpClient = _httpClientFactory.CreateClient(_httpClientName);

        var request = new HttpRequestMessage(HttpMethod.Get,_ipRequestAddress);

        HttpResponseMessage response = httpClient.Send(request);
        string responseString = response.Content.ReadAsStringAsync().Result;
        string address =
            AddressFromResponseExtractor.ExtractAddressFromResponse(responseString);

        return new EmailData()
        {
            EmailSubject = "Ip By Email Service",
            EmailText = address
        };
    }
}