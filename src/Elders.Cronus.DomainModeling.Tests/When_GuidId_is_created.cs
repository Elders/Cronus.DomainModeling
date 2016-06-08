using System;
using Machine.Specifications;

namespace Elders.Cronus.DomainModeling.Tests
{
    [Subject("GuidId")]
    public class When_GuidId_is_created
    {
        Establish context = () =>
        {
            id = "d79e69ef-b0cf-48d9-b400-6e40877e3272";
            basePart = string.Empty;
            aggregateName = "arName";
            valuePart = aggregateName + ":" + id;
            urn = new Urn(basePart, valuePart);
        };

        Because of = () => result = new GuidId(Guid.Parse(id), aggregateName);

        It should_have_value_as_base_part = () => result.Urn.BasePart.ShouldEqual(urn.BasePart);

        It should_have_value_as_value_part = () => result.Urn.ValuePart.ShouldEqual(urn.ValuePart);

        It should_have_value = () => result.Urn.Value.ShouldEqual(urn.Value);

        static IAggregateRootId result;
        static IUrn urn;
        static string basePart;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}
