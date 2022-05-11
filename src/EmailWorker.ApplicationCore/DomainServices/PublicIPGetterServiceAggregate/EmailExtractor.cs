using System;

namespace EmailWorker.ApplicationCore.DomainServices.PublicIPGetterServiceAggregate;

public class EmailExtractor
{
    public static string ExtractEmail(string rawString)
    {
        int first = rawString.IndexOf('<');
        int last = rawString.IndexOf('>');

        if (first < 0 || last < 0)
        {
            throw new FormatException("The argument string is in invalid format!");
        }

        string emailFrom;

        emailFrom = rawString.Substring(first + 1, last - first - 1);
        return emailFrom;
    }
}