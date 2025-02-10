namespace Elders.Cronus;

public class TenantUrnIsCreated
{
    static Urn result;
    static Urn urn;
    static string tenant;
    static string valuePart;
    static string aggregateName;
    static string id;

    [Before(Class)]
    public static void Setup()
    {
        id = "123";
        tenant = "tenant";
        aggregateName = "arName";
        valuePart = aggregateName + ":" + id;
        urn = new Urn(tenant, valuePart);
        result = new Urn(tenant, valuePart);
    }

    [Test]
    public async Task ShouldHaveTenantBasePart()
    {
        await Assert.That(result.NID).IsEqualTo(urn.NID);
    }

    [Test]
    public async Task ShouldHaveValueAsValuePart()
    {
        await Assert.That(result.NSS).IsEqualTo(urn.NSS);
    }

    [Test]
    public async Task ShouldHaveValue()
    {
        await Assert.That(result.Value).IsEqualTo(urn.Value);
    }
}
