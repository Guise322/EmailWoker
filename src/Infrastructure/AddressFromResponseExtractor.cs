//Some of the below lines of code is got from the site:
//https://qawithexperts.com/article/c-sharp/get-ip-address-using-c-local-and-public-ip-example/374

namespace EmailWorker.Infrastructure;

public static class AddressFromResponseExtractor
{
    public static string ExtractAddressFromResponse(string response)
    {
        int first = response.IndexOf("Address: ");
        int last = response.LastIndexOf("</body>");

        if (first < 0 || last < 0)
        {
            throw new FormatException("The response message is in invalid format.");
        }
        int stringOffset = 9;
        first += stringOffset;
        return response.Substring(first, last - first);
    }
}