using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("StringTenatnId")]
    public class When_StringTenatnId_is_created
    {
        Establish context = () =>
        {
            id = "123";
            tenant = "tenant";
            aggregateName = "arname";
            valuePart = aggregateName + ":" + id;
            urn = new Urn(tenant, valuePart);
        };

        Because of = () => result = new AggregateRootId(tenant, aggregateName, id);

        It should_have_tenant_as_base_part = () => result.NID.ShouldEqual(urn.NID);

        It should_have_the_same_value_part = () => result.NSS.ShouldEqual(urn.NSS);

        static Urn urn;
        static AggregateRootId result;
        static string tenant;
        static string valuePart;
        static string aggregateName;
        static string id;
    }
}
