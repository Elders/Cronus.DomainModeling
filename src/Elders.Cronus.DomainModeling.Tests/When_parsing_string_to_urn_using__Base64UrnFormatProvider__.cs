using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_string_to_urn_using__Base64UrnFormatProvider__
    {
        Because of = () => result = Urn.Parse(urnBase64, provider);

        It should_build_urn = () => result.Value.ShouldEqual(urn.Value);

        static Base64UrnFormatProvider provider = new Base64UrnFormatProvider();
        static IUrn urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static string urnBase64 = "dXJuOnRlbmFudDphcm5hbWU6YWJjMTIzKCkrLC0uOj1AOyRfISonJTk5YQ==";
        static IUrn result;
    }
}
