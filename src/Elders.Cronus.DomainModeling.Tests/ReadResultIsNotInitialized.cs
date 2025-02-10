namespace Elders.Cronus;

public class ReadResultIsNotInitialized
{
    static ReadResult<string> result;

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
    public async Task ShouldHaveNegativeHasError()
    {
        await Assert.That(result.HasError).IsFalse();
    }
}
