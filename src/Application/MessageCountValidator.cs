namespace EmailWorker.Application;
public static class MessageCountValidator
{
    public static bool IsMessageCountValid(int messageCount)
    {
        int minCount = 5;

        return messageCount < minCount;
    }
}