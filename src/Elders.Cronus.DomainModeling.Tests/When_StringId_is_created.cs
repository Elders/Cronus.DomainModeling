using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
{
    [Subject("StringTenatnId")]
    public class When_StringId_is_created
    {
        Establish context = () =>
        {
            id = "123";
            basePart = string.Empty;
            aggregateName = "arName";
            valuePart = aggregateName + ":" + id;
            urn = new Urn(basePart, valuePart);
        };

        Because of = () => result = new StringId(id, aggregateName);

        It should_have_tenant_as_base_part = () => result.Urn.BasePart.ShouldEqual(urn.BasePart);

        It should_have_the_same_value_part = () => result.Urn.ValuePart.ShouldEqual(urn.ValuePart);

        static IUrn urn;
        static IAggregateRootId result;
        static string basePart;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}