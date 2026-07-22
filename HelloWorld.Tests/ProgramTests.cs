using Xunit;

public class ProgramTests
{
    [Fact]
    public void GetGreeting_ReturnsHelloWorld()
    {
        Assert.Equal("Hello, World!", Program.GetGreeting());
    }
}
