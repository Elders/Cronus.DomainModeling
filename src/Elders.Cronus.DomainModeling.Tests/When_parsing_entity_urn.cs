using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_entity_urn
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arname:123/entityname:entityid";
            aggUrn = new AggregateUrn("tenant", "arname", "123");
        };

        Because of = () => result = new EntityUrn(aggUrn, "entityname", "entityId");

        It should_have_correct_urn = () => result.ToString().ShouldEqual(urn);

        It should_have_correct_entity_name = () => result.EntityName.ToString().ShouldEqual("entityname");

        It should_have_correct_entity_id = () => result.EntityId.ShouldEqual("entityid");

        static AggregateUrn aggUrn;
        static string urn;
        static EntityUrn result;
    }
}
