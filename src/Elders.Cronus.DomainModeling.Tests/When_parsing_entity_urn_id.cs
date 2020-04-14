using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_entity_urn_id
    {
        Establish context = () =>
        {
            urnString = "urn:tenant:arName:123a/Entity:456E";
        };

        Because of = () => result = EntityUrn.Parse(urnString);

        It should_have_tenant_as_base_part = () => result.ShouldNotBeNull();

        static string urnString;
        static EntityUrn result;
    }
}
