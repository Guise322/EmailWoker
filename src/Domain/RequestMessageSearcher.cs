namespace EmailWorker.Domain;

//TO DO: implement DI and create an interface for the class

public static class RequestMessageSearcher
{
    public static bool IsRequestMessage(string rawEmailFrom, string searchedEmail)
    {
        string emailFrom = EmailExtractor.ExtractEmail(rawEmailFrom);
        return emailFrom == searchedEmail;
    }
}