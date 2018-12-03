using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_checking_string_is_urn_using__Base64UrnFormatProvider__
    {
        Because of = () => result = Urn.IsUrn(urnBase64, provider);

        It should_build_urn = () => result.ShouldBeTrue();

        static Base64UrnFormatProvider provider = new Base64UrnFormatProvider();
        static string urnBase64 = "dXJuOnRlbmFudDphcm5hbWU6YWJjMTIzKCkrLC0uOj1AOyRfISonJTk5YQ==";
        static bool result;
    }
}
