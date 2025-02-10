namespace Elders.Cronus;

public class ComparingTwoEntityIds
{
    static Urn urn;
    static AggregateRootId origin;
    static EntityId<AggregateRootId> result;

    [Before(Class)]
    public static void Setup()
    {
        urn = new Urn("urn:tenant:arName:123a/Entity:456E".ToLower());
        origin = new AggregateRootId("Tenant", "arName", "123a");
        result = new TestEntityUrnId("456E", origin);
    }

    [Test]
    public async Task ShouldBeEqual()
    {
        await Assert.That(result == urn).IsTrue();
    }
}
