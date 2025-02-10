namespace Elders.Cronus;

public class ReadResultIsSuccessful
{
    static ReadResult<int> result;

    [Before(Class)]
    public static void Setup()
    {
        result = new ReadResult<int>(123);
    }

    [Test]
    public async Task ShouldHavePositiveIsSuccess()
    {
        await Assert.That(result.IsSuccess).IsTrue();
    }

    [Test]
    public async Task ShouldHaveData()
    {
        await Assert.That(result.Data).IsEqualTo(123);
    }

    [Test]
    public async Task ShouldHaveNegativeNotFound()
    {
        await Assert.That(result.NotFound).IsFalse();
    }

    [Test]
    public async Task ShouldHaveNegativeHasError()
    {
        await Assert.That(result.HasError).IsFalse();
    }
}
