using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_string_to_urn_using__Base64UrlTokenUrnFormatProvider__
    {
        Because of = () => result = Urn.Parse(urnBase64UrlToken, provider);

        It should_build_urn = () => result.Value.ShouldEqual(urn.Value);

        static Base64UrlTokenUrnFormatProvider provider = new Base64UrlTokenUrnFormatProvider();
        static IUrn urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static string urnBase64UrlToken = "dXJuOnRlbmFudDphcm5hbWU6YWJjMTIzKCkrLC0uOj1AOyRfISonJTk5YQ2";
        static IUrn result;
    }
}
