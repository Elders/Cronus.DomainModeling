namespace Elders.Cronus;

public class ParsingEntityUrnId
{
    static string urnString;
    static EntityId result;

    [Before(Class)]
    public static void Setup()
    {
        urnString = $"urn:tenant:arname:randomid/entityname:randomguid:somethingelse";
        result = EntityId.Parse(urnString);
    }

    [Test]
    public async Task ShouldHaveUrnInstance()
    {
        await Assert.That(result).IsNotNull();
    }

    [Test]
    public async Task ShouldHaveProperAggregateId()
    {
        await Assert.That(result.AggregateRootId.Value).IsEqualTo("urn:tenant:arname:randomid", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ShouldHaveProperEntityId()
    {
        await Assert.That(result.EntityID).IsEqualTo("randomguid:somethingelse", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ShouldHaveProperValueAsString()
    {
        await Assert.That(result.Value).IsEqualTo(urnString, StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ShouldHaveProperNid()
    {
        await Assert.That(result.NID).IsEqualTo("tenant", StringComparison.OrdinalIgnoreCase);
    }
}
