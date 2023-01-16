using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_formatting_urn_using__Base64UrnFormatProvider__
    {
        Establish context = () =>
        {
            Urn.UseCaseSensitiveUrns = true;

            urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        };

        Because of = () => result = provider.Format(urn);

        It should_format_to_base64 = () => result.ShouldEqual(urnBase64);

        static Base64UrnFormatProvider provider = new Base64UrnFormatProvider();
        static Urn urn;
        static string urnBase64 = "dXJuOlRlbmFudDphck5hbWU6YWJjMTIzKCkrLC0uOj1AOyRfISonJTk5YQ==";
        static string result;

        Cleanup after = () => Urn.UseCaseSensitiveUrns = false;
    }
}
