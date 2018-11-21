using Machine.Specifications;

namespace Elders.Cronus
{
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
