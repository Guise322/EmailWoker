using EmailWorker.Application;
using Xunit;

namespace EmailWorker.Tests.Unit.Application;

public class MessageCountValidatorTests
{
    [Theory]
    [InlineData(4)]
    [InlineData(5)]
    public void IsMessageCountValid_NumbersBelowLimit_False(int value)
    {
        var actualResult = MessageCountValidator.IsMessageCountValid(value);
        Assert.False(actualResult);
    }

    [Theory]
    [InlineData(6)]
    public void IsMessageCountValid_NumbersAboveLimit_True(int value)
    {
        var actualResult = MessageCountValidator.IsMessageCountValid(value);
        Assert.True(actualResult);
    }
}