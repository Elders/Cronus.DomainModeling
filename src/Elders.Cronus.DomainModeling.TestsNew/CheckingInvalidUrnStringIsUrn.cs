namespace Elders.Cronus;

public class CheckingInvalidUrnStringIsUrn
{
    static bool result;

    [Before(Class)]
    public static void Startup()
    {
        result = Urn.IsUrn("invalid");
    }

    [Test]
    public async Task ShouldBeNotValid()
    {
        await Assert.That(result).IsFalse();
    }
}
