using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When__Urn__is_created
    {
        Establish context = () =>
        {
            urn = "urn:tenant:arName:123";
        };

        Because of = () => result = Urn.Parse(urn);

        It should_have_tenant_as_base_part = () => result.NID.ShouldEqual("tenant");

        It should_have_value_as_value_part = () => result.NSS.ShouldEqual("arName:123");

        It should_have_value = () => result.Value.ShouldEqual("urn:tenant:arName:123");

        static IUrn result;
        static string urn;
    }
}
