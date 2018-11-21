using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_string_to_urn_using__PlainUrnFormatProvider__
    {
        Because of = () => result = Urn.Parse(urnPlain, provider);

        It should_build_urn = () => result.Value.ShouldEqual(urn.Value);

        static PlainUrnFormatProvider provider = new PlainUrnFormatProvider();
        static IUrn urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static string urnPlain = @"urn:tenant:arname:abc123()+,-.:=@;$_!*'%99a";
        static IUrn result;
    }
}
