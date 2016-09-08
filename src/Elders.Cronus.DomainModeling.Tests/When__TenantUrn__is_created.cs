using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
{
    [Subject("Urn")]
    public class When__Urn__is_created
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arName:123";
        };

        Because of = () => result = Urn.Parse(urn);

        It should_have_tenant_as_base_part = () => result.NID.ShouldEqual("tenant");

        It should_have_value_as_value_part = () => result.NSS.ShouldEqual("arname:123");

        It should_have_value = () => result.Value.ShouldEqual("urn:tenant:arname:123");

        static IUrn result;
        static string urn;
    }

    [Subject("Urn")]
    public class When_building_aggregate_id_with_urn
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arname:123";
            origin = new StringTenantId("123", "arName", "tenant");
        };

        Because of = () => result = new StringTenantId(Urn.Parse(urn), "arname");

        It should_have_tenant_as_base_part = () => result.ToString().ShouldEqual(origin.ToString());

        static string urn;
        static IAggregateRootId origin;
        static IAggregateRootId result;
    }

    [Subject("Urn")]
    public class When_building_entity_id_with_urn
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arname:123a:entity:456e";
            origin = new StringTenantId("123A", "arName", "Tenant");
        };

        Because of = () => result = new EntityStringId<StringTenantId>("456E", origin, "Entity");

        It should_have_tenant_as_base_part = () => result.Urn.Value.ShouldEqual(urn);

        static string urn;
        static StringTenantId origin;
        static EntityStringId<StringTenantId> result;
    }
}
