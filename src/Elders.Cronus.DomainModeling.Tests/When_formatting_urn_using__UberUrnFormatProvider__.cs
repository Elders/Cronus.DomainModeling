using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_formatting_urn_using__UberUrnFormatProvider__
    {
        Establish context = () =>
        {
            Urn.UseCaseSensitiveUrns = true;

            urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        };

        Because of = () => result = provider.Format(urn);

        It should_format_to_base64 = () => result.ShouldEqual(urnBase64UrlToken);

        static UberUrnFormatProvider provider = new UberUrnFormatProvider();
        static IUrn urn;
        static string urnBase64UrlToken = @"dXJuOlRlbmFudDphck5hbWU6YWJjMTIzKCkrLC0uOj1AOyRfISonJTk5YQ2";
        static string result;

        Cleanup after = () => Urn.UseCaseSensitiveUrns = false;
    }
}
