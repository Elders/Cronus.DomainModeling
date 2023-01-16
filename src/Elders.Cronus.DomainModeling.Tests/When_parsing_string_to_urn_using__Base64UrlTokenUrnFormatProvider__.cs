using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_string_to_urn_using__Base64UrlTokenUrnFormatProvider__
    {
        Because of = () => result = Urn.Parse("urn:Tenant:arName:abc123()+,-.:=@;$_!*'%99a");

        It should_build_urn = () => result.Value.ShouldEqual(urn.Value);

        static Base64UrlTokenUrnFormatProvider provider = new Base64UrlTokenUrnFormatProvider();
        static Urn urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static Urn result;
    }
}
