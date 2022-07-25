namespace EmailWorker.Domain;

internal static class EmailExtractor
{
    internal static string ExtractEmail(string rawString)
    {
        int first = rawString.IndexOf('<');
        int last = rawString.IndexOf('>');

        if (first < 0 || last < 0)
        {
            throw new FormatException("The argument string is in invalid format!");
        }

        return rawString.Substring(first + 1, last - first - 1);
    }
}