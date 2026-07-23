using Xunit;

public class ProgramTests
{
    [Fact]
    public void GetGreeting_ReturnsWelcomeMessage()
    {
        Assert.Equal("Welcome to EFD C# project!", Program.GetGreeting());
    }
}
