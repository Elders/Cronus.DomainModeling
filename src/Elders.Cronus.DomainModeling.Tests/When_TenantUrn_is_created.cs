using Machine.Specifications;

namespace Elders.Cronus.Tests
{
    [Subject("TenantUrn")]
    public class When_TenantUrn_is_created
    {
        Establish context = () =>
        {
            id = "123";
            tenant = "tenant";
            aggregateName = "arName";
            valuePart = aggregateName + ":" + id;
            urn = new Urn(tenant, valuePart);
        };

        Because of = () => result = new Urn(urn);

        It should_have_tenant_as_base_part = () => result.NID.ShouldEqual(urn.NID);

        It should_have_value_as_value_part = () => result.NSS.ShouldEqual(urn.NSS);

        It should_have_value = () => result.Value.ShouldEqual(urn.Value);

        static IUrn result;
        static IUrn urn;
        static string tenant;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}
