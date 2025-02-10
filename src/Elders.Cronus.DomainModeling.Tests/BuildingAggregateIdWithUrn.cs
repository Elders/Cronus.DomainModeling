namespace Elders.Cronus;

public class BuildingAggregateIdWithUrn
{
    static AggregateRootId urn;
    static AggregateRootId origin;
    static AggregateRootId result;

    [Before(Class)]
    public static void Setup()
    {
        urn = AggregateRootId.Parse("urn:tenant:arname:123");
        origin = new AggregateRootId("tenant", "arname", "123");

        result = new AggregateRootId(urn);
    }

    [Test]
    public async Task ResultMustBeEqualToUrn()
    {
        await Assert.That(result).IsEqualTo(origin);
    }

    [Test]
    public async Task ResultMustBeEqualToOrigin()
    {
        await Assert.That(result).IsEqualTo(urn);
    }
}
