using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_formatting_urn_using__PlainUrnFormatProvider__
    {
        Establish context = () =>
        {
            Urn.UseCaseSensitiveUrns = true;

            urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        };

        Because of = () => result = provider.Format(urn);

        It should_format_to_base64 = () => result.ShouldEqual(urnPlain);

        static PlainUrnFormatProvider provider = new PlainUrnFormatProvider();
        static IUrn urn;
        static string urnPlain = @"urn:Tenant:arName:abc123()+,-.:=@;$_!*'%99a";
        static string result;

        Cleanup after = () => Urn.UseCaseSensitiveUrns = false;
    }
}
