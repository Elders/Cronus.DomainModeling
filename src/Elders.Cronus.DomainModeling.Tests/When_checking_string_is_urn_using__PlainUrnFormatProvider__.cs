using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_checking_string_is_urn_using__PlainUrnFormatProvider__
    {
        Because of = () => result = Urn.IsUrn(urnPlain, provider);

        It should_build_urn = () => result.ShouldBeTrue();

        static PlainUrnFormatProvider provider = new PlainUrnFormatProvider();
        static string urnPlain = @"urn:tenant:arname:abc123()+,-.:=@;$_!*'%99a";
        static bool result;
    }
}
