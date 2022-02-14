//Some of the below lines of code is got from the site:
//https://qawithexperts.com/article/c-sharp/get-ip-address-using-c-local-and-public-ip-example/374

using System.Collections.Generic;
using System.Net.Http;
using EmailWorker.ApplicationCore.Entities;
using EmailWorker.ApplicationCore.Interfaces.HandlersOfProcessedMessages;
using MailKit;

namespace EmailWorker.Infrastructure.HandlersOfProcessedMessages;

public class HandlerOfPublicIPGetterMessages : IHandlerOfPublicIPGetterMessages
{
    private readonly string ipRequestAddress = "http://checkip.dyndns.org/";
    
    private readonly IHttpClientFactory _httpClientFactory;
    public HandlerOfPublicIPGetterMessages(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;
    public EmailData HandleProcessedMessages(IList<UniqueId> messages)
    {
        HttpRequestMessage request = new(HttpMethod.Get,ipRequestAddress);

        string httpClientName = "checkip";

        //TO DO: add checking if the request was successful

        HttpClient httpClient = _httpClientFactory.CreateClient(httpClientName);
        HttpResponseMessage response = httpClient.Send(request);
        string address = response.Content.ReadAsStringAsync().Result;
        
        int first = address.IndexOf("Address: ") + 9;
        int last = address.LastIndexOf("</body>");
        address = address.Substring(first, last - first);
        
        //_logger.LogInformation("Getting of the current IP succeeds.");

        return new EmailData()
        {
            EmailSubject = "Ip By Email Project",
            EmailText = address
        };
    }
}