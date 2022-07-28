namespace EmailWorker.Application;
internal static class MessageCountValidator
{
    internal static bool IsMessageCountValid(int messageCount)
    {
        int minCount = 5;

        return minCount < messageCount;
    }
}