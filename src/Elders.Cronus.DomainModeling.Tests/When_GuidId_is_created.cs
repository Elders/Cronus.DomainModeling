using System;
using Machine.Specifications;

namespace Elders.Cronus.Tests
{
    [Subject("GuidId")]
    public class When_GuidId_is_created
    {
        Establish context = () =>
        {
            id = "d79e69ef-b0cf-48d9-b400-6e40877e3272";
            NID = "arName";
            aggregateName = "arName";
            valuePart = id;
            urn = new Urn(NID, valuePart);
        };

        Because of = () => result = new GuidId(Guid.Parse(id), aggregateName);

        It should_have_value_as_base_part = () => result.Urn.NID.ShouldEqual(urn.NID);

        It should_have_value_as_value_part = () => result.Urn.NSS.ShouldEqual(urn.NSS);

        It should_have_value = () => result.Urn.Value.ShouldEqual(urn.Value);

        static IAggregateRootId result;
        static IUrn urn;
        static string NID;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}
