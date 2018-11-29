using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("StringTenatnId")]
    public class When_StringId_is_created
    {
        Establish context = () =>
        {
            id = "123";
            NID = "arName";
            aggregateName = "arName";
            valuePart = id;
            urn = new Urn(NID, valuePart);
        };

        Because of = () => result = new StringId(id, aggregateName);

        It should_have_tenant_as_base_part = () => result.Urn.NID.ShouldEqual(urn.NID);

        It should_have_the_same_value_part = () => result.Urn.NSS.ShouldEqual(urn.NSS);

        static IUrn urn;
        static IAggregateRootId result;
        static string NID;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}
