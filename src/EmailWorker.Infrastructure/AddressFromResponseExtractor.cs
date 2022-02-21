//Some of the below lines of code is got from the site:
//https://qawithexperts.com/article/c-sharp/get-ip-address-using-c-local-and-public-ip-example/374

namespace EmailWorker.Infrastructure;

public static class AddressFromResponseExtractor
{
    public static string ExtractAddressFromResponse(string response)
    {
        int first = response.IndexOf("Address: ") + 9;
        int last = response.LastIndexOf("</body>");
        return response.Substring(first, last - first);
    }
}