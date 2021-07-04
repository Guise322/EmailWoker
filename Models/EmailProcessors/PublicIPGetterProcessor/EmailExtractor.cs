namespace EmailWorker.Controllers.EmailProcessors.PublicIPGetterProcessor
{
    public class EmailExtractor
    {
        public static string ExtractEmail(string rawString)
        {
            int first = rawString.IndexOf('<');
            int last = rawString.IndexOf('>');

            string emailFrom;

            if (first !> 0)
            {
                emailFrom = rawString;
                return emailFrom;    
            }

            emailFrom = rawString.Substring(first + 1, last - first - 1);
            return emailFrom;
        }
    }
}