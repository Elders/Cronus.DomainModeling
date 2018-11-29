using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_building_aggregate_id_with_urn
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arname:123";
            origin = new StringTenantId("123", "arName", "tenant");
        };

        Because of = () => result = new StringTenantId(StringTenantUrn.Parse(urn), "arName");

        It should_have_tenant_as_base_part = () => result.ToString().ShouldEqual(origin.ToString());

        static string urn;
        static IAggregateRootId origin;
        static IAggregateRootId result;
    }
}
