using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_building_aggregate_id_with_urn
    {
        Establish context = () =>
        {
            urn = AggregateRootId.Parse("urn:tenant:arname:123");
            origin = new AggregateRootId("tenant", "arname", "123");
        };

        Because of = () => result = new AggregateRootId(urn);

        It should_have_tenant_as_base_part = () => result.ShouldEqual(origin);

        static AggregateRootId urn;
        static AggregateRootId origin;
        static AggregateRootId result;
    }
}
