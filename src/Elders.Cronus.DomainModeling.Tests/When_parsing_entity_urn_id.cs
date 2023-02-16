using Machine.Specifications;

namespace Elders.Cronus
{
    /// <summary>
    /// Mynkow: "Testovete shte pokajat" :D
    /// </summary>
    [Subject("Urn")]
    public class When_parsing_entity_urn_id
    {
        Establish context = () =>
        {
            aggregateRootId = "urn:tenant:arName:123a";
            urnString = $"{aggregateRootId}/Entity:456E";
        };

        Because of = () => result = EntityId.Parse(urnString);

        It should_have_urn_instance = () => result.ShouldNotBeNull();

        It should_have_proper_aggregate_id = () => result.AggregateRootId.Value.ShouldBeEqualIgnoringCase(aggregateRootId);

        It should_have_proper_entity_name = () => result.EntityName.ShouldBeEqualIgnoringCase("Entity");

        It should_have_proper_entity_id = () => result.EntityID.ShouldBeEqualIgnoringCase("456E");

        It should_have_proper_value_as_string = () => result.Value.ShouldBeEqualIgnoringCase(urnString);

        It should_have_proper_nid = () => result.NID.ShouldBeEqualIgnoringCase("tenant");

        static string urnString;
        static EntityId result;
        static string aggregateRootId;
    }
}
