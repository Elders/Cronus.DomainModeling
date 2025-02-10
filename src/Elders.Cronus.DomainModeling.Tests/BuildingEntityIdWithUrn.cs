namespace Elders.Cronus;

public class BuildingEntityIdWithUrn
{
    static AggregateRootId origin;
    static EntityId<AggregateRootId> result;

    [Before(Class)]
    public static void Setup()
    {
        origin = new AggregateRootId("Tenant", "arName", "123a");
        result = new TestEntityUrnId("456E", origin);
    }

    [Test]
    public async Task ShouldHaveTenantAsBasePart()
    {
        await Assert.That(result.AggregateRootId.Tenant).IsEqualTo("Tenant", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ShouldHaveAggregateReference()
    {
        await Assert.That((Urn)result.AggregateRootId == origin).IsTrue();
    }

    [Test]
    public async Task ShouldHaveId()
    {
        await Assert.That(result.Id).IsEqualTo("arname:123a/entity:456e");
    }

    [Test]
    public async Task ShouldHaveEntityId()
    {
        await Assert.That(result.EntityID).IsEqualTo("456e");
    }
}
