namespace Elders.Cronus;

public class ResolvingBoundedContextFromAMessageWithoutDataContractAttribute
{
    static string result;

    [Before(Class)]
    public static void Setup()
    {
        result = typeof(IPublicEvent).GetBoundedContext("elders");
    }

    [Test]
    public async Task ShouldResolveTheDefaultBoundedContext()
    {
        await Assert.That(result).IsEqualTo("elders");
    }
}
