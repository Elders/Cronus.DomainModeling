using Machine.Specifications;

namespace Elders.Cronus;

[Subject("Urn")]
public class When_parsing_entity_urn
{
    Establish context = () =>
    {
        urn = "urn:tenant:arname:123/entityname:entityid";
        aggUrn = new AggregateRootId("tenant", "arname", "123");
    };

    Because of = () => result = new EntityId(aggUrn, "entityname", "entityId");

    It should_have_correct_urn = () => result.ToString().ShouldEqual(urn);

    It should_have_correct_entity_id = () => result.EntityID.ShouldEqual("entityid");

    static AggregateRootId aggUrn;
    static string urn;
    static EntityId result;
}
