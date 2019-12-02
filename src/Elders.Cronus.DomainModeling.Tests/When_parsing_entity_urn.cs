using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_entity_urn
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arname:123/entityname/entityid";
            aggUrn = new StringTenantUrn("tenant", "arname", "123");
        };

        Because of = () => result = new StringTenantEntityUrn(aggUrn, "entityname", "entityId");

        It should_have_correct_urn = () => result.ToString().ShouldEqual(urn);

        It should_have_correct_entity_name = () => result.EntityName.ToString().ShouldEqual("entityname");

        It should_have_correct_entity_id = () => result.Id.ShouldEqual("entityid");

        static StringTenantUrn aggUrn;
        static string urn;
        static StringTenantEntityUrn result;
    }
}
