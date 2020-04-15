using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_building_entity_id_with_urn
    {
        Establish context = () =>
        {
            origin = new AggregateRootId("123a", "arName", "Tenant");
        };

        Because of = () => result = new TestEntityId("456E", origin, "Entity");

        It should_have_tenant_as_base_part = () => result.AggregateRootId.Tenant.ShouldBeEqualIgnoringCase("Tenant");

        It should_have_aggregate_reference = () => ((Urn)result.AggregateRootId == origin).ShouldBeTrue();

        It should_have_id = () => result.Id.ShouldEqual("arname:123a/entity:456e");

        It should_have_entity_id = () => result.EntityId.ShouldEqual("456e");

        It should_have_entity_name = () => result.EntityName.ShouldEqual("entity");

        static AggregateRootId origin;
        static EntityId<AggregateRootId> result;
    }
}
