using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
{
    [Subject("Urn")]
    public class When__TenantUrn__is_created
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arName:123";
        };

        Because of = () => result = new TenantUrn(urn);

        It should_have_tenant_as_base_part = () => result.BasePart.ShouldEqual("tenant");

        It should_have_value_as_value_part = () => result.ValuePart.ShouldEqual("arName:123");

        It should_have_value = () => result.Value.ShouldEqual("urn:tenant:arName:123");

        static IUrn result;
        static string urn;
    }

    [Subject("Urn")]
    public class When_building_aggregate_id_with_urn
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arName:123";
            origin = new StringTenantId("123", "arName", "tenant");
        };

        Because of = () => result = new StringTenantId(new Urn(urn), "arname");

        It should_have_tenant_as_base_part = () => result.ToString().ShouldEqual(origin.ToString());

        static string urn;
        static IAggregateRootId origin;
        static IAggregateRootId result;
    }
}
