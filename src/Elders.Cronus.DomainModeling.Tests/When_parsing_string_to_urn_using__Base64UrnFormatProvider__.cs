using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_string_to_urn_using__Base64UrnFormatProvider__
    {
        Because of = () => result = Urn.Parse(urnBase64, provider);

        It should_build_urn = () => result.Value.ShouldEqual(urn.Value);

        static Base64UrnFormatProvider provider = new Base64UrnFormatProvider();
        static Urn urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static string urnBase64 = "dXJuOlRlbmFudDphck5hbWU6YWJjMTIzKCkrLC0uOj1AOyRfISonJTk5YQ==";
        static Urn result;
    }

    [Subject("Urn")]
    public class When_parsing_string_to__AggregateUrn__
    {
        Because of = () => result = AggregateRootId.Parse(urnString);

        It should_build_urn = () => result.Value.ShouldEqual("urn:elders:arname:something:3");

        It should_have_nid = () => result.NID.ShouldEqual("elders");

        It should_have_nss = () => result.NSS.ShouldEqual("arname:something:3");

        It should_have_aggregate_name = () => result.AggregateRootName.ShouldEqual("arname");

        It should_have_tenant = () => result.Tenant.ShouldEqual("elders");

        It should_have_id = () => result.Id.ShouldEqual("something:3");

        static string urnString = "urn:elders:arname:something:3";
        static AggregateRootId result;
    }
}
