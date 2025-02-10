namespace Elders.Cronus;

public class ReadResultToString
{
    static ReadResult<string> result;

    [Test]
    public async Task ShouldNotThrow()
    {
        await Assert.That(() => result.ToString().StartsWith("ReadResult<String> => IsSuccess:")).IsTrue();
    }
}
