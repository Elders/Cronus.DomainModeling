using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
{
    [Subject("Urn")]
    public class When_Urn_is_created
    {
        Establish context = () =>
        {
            id = "123";
            aggregateName = "arName";
            valuePart = aggregateName + ":" + id;
            basePart = string.Empty;
            urn = "urn:arname:123";
        };

        Because of = () => result = new Urn(basePart, valuePart);

        It should_have_value_as_value_part = () => result.ValuePart.ShouldEqual("arname:123");

        It should_have_value = () => result.Value.ShouldEqual(urn);

        static IUrn result;
        static string urn;
        static string basePart;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}
