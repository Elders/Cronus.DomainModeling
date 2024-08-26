using Machine.Specifications;

namespace Elders.Cronus;

/// <summary>
/// Mynkow: "Testovete shte pokajat" :D
/// </summary>
[Subject("Urn")]
public class When_parsing_entity_urn_id
{
    Establish context = () =>
    {
        urnString = $"urn:tenant:arname:randomid/entityname:randomguid:somethingelse";
    };

    Because of = () => result = EntityId.Parse(urnString);

    It should_have_urn_instance = () => result.ShouldNotBeNull();

    It should_have_proper_aggregate_id = () => result.AggregateRootId.Value.ShouldBeEqualIgnoringCase("urn:tenant:arname:randomid");

    It should_have_proper_entity_id = () => result.EntityID.ShouldBeEqualIgnoringCase("randomguid:somethingelse");

    It should_have_proper_value_as_string = () => result.Value.ShouldBeEqualIgnoringCase(urnString);

    It should_have_proper_nid = () => result.NID.ShouldBeEqualIgnoringCase("tenant");

    static string urnString;
    static EntityId result;
}
