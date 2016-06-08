using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
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

        Because of = () => result = new TenantUrn(urn);

        It should_have_tenant_as_base_part = () => result.BasePart.ShouldEqual(urn.BasePart);

        It should_have_value_as_value_part = () => result.ValuePart.ShouldEqual(urn.ValuePart);

        It should_have_value = () => result.Value.ShouldEqual(urn.Value);

        static IUrn result;
        static IUrn urn;
        static string tenant;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}