using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_building_aggregate_id_with_urn
    {
        Establish context = () =>
        {
            urn = AggregateUrn.Parse("urn:tenant:arname:123");
            origin = new AggregateRootId("123", "arname", "tenant");
        };

        Because of = () => result = new AggregateRootId("arName", urn);

        It should_have_tenant_as_base_part = () => result.ShouldEqual(origin);

        static AggregateUrn urn;
        static IAggregateRootId origin;
        static IAggregateRootId result;
    }
}
