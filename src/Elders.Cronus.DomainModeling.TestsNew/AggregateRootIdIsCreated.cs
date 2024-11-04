using Elders.Cronus.EntityTests.TestModel;
using Elders.Cronus.Testing;

namespace Elders.Cronus;

public class AggregateRootIdIsCreated
{
    static Urn urn;
    static AggregateRootId result;
    static string tenant;
    static string valuePart;
    static string aggregateName;
    static string id;

    [Before(Class)]
    public static void Setup()
    {
        id = "123";
        tenant = "tenant";
        aggregateName = "arname";
        valuePart = aggregateName + ":" + id;
        urn = new Urn(tenant, valuePart);
        result = new AggregateRootId(tenant, aggregateName, id);
    }

    [Test]
    public async Task ShouldHaveTenantAsBasePart()
    {
        await Assert.That(result.NID).IsEqualTo(urn.NID, StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ShouldHaveTheSameValuePart()
    {
        await Assert.That(result.NSS).IsEqualTo(urn.NSS);
    }
}
