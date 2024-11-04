namespace Elders.Cronus;

public class UrnIsCreated
{
    static Urn urn;

    [Before(Class)]
    public static void Setup()
    {
        Urn.UseCaseSensitiveUrns = true;

        urn = new Urn("urn:tenant:arName:123");
    }

    [After(Class)]
    public static void Cleanup()
    {
        Urn.UseCaseSensitiveUrns = false;
    }

    [Test, NotInParallel]
    public async Task ShouldHaveBasePart()
    {
        await Assert.That(urn.NID).IsEqualTo("tenant");
    }

    [Test, NotInParallel]
    public async Task ShouldHaveValue()
    {
        await Assert.That(urn.NSS).IsEqualTo("arName:123");
    }

    [Test, NotInParallel]
    public async Task ShouldHaveCorrectToString()
    {
        await Assert.That(urn.ToString()).IsEqualTo("urn:tenant:arName:123");
    }
}
