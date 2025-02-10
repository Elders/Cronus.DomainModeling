namespace Elders.Cronus;

public class ReadResultHasError
{
    static ReadResult<string> result;

    [Before(Class)]
    public static void Setup()
    {
        result = ReadResult<string>.WithError("error");
    }

    [Test]
    public async Task ShouldHaveNegativeIsSuccess()
    {
        await Assert.That(result.IsSuccess).IsFalse();
    }

    [Test]
    public async Task ShouldHaveNullData()
    {
        await Assert.That(result.Data).IsNull();
    }

    [Test]
    public async Task ShouldHavePositiveNotFound()
    {
        await Assert.That(result.NotFound).IsTrue();
    }

    [Test]
    public async Task ShouldHavePositiveHasError()
    {
        await Assert.That(result.HasError).IsTrue();
    }
}
