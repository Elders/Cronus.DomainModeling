namespace Elders.Cronus;

public class ParsingEntityUrn
{
    static AggregateRootId aggUrn;
    static string urn;
    static EntityId result;

    [Before(Class)]
    public static void Setup()
    {
        urn = "urn:tenant:arname:123/entityname:entityid";
        aggUrn = AggregateRootId.Parse("urn:tenant:arname:123");
        result = new EntityId(aggUrn, "entityname", "entityId");
    }

    [Test]
    public async Task ShouldHaveCorrectUrn()
    {
        await Assert.That(result.ToString()).IsEqualTo(urn);
    }

    [Test]
    public async Task ShouldHaveCorrectEntityId()
    {
        await Assert.That(result.EntityID).IsEqualTo("entityid");
    }
}
