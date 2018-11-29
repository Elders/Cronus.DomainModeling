using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_formatting_urn_using__PlainUrnFormatProvider__
    {
        Because of = () => result = provider.Format(urn);

        It should_format_to_base64 = () => result.ShouldEqual(urnPlain);

        static PlainUrnFormatProvider provider = new PlainUrnFormatProvider();
        static IUrn urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static string urnPlain = @"urn:tenant:arname:abc123()+,-.:=@;$_!*'%99a";
        static string result;
    }
}
