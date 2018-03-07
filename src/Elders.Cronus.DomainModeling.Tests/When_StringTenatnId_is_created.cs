using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
{
    [Subject("StringTenatnId")]
    public class When_StringTenatnId_is_created
    {
        Establish context = () =>
        {
            id = "123";
            tenant = "tenant";
            aggregateName = "arName";
            valuePart = aggregateName + ":" + id;
            urn = new Urn(tenant, valuePart);
        };

        Because of = () => result = new StringTenantId(id, aggregateName, tenant);

        It should_have_tenant_as_base_part = () => result.Urn.NID.ShouldEqual(urn.NID);

        It should_have_the_same_value_part = () => result.Urn.NSS.ShouldEqual(urn.NSS);

        static IUrn urn;
        static IAggregateRootId result;
        static string tenant;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}
