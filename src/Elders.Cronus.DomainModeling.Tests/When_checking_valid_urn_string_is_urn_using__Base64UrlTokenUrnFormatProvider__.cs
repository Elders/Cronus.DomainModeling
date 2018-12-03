using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_checking_valid_urn_string_is_urn_using__Base64UrlTokenUrnFormatProvider__
    {
        Because of = () => result = Urn.IsUrn(urnBase64UrlToken, provider);

        It should_be_true = () => result.ShouldBeTrue();

        static Base64UrlTokenUrnFormatProvider provider = new Base64UrlTokenUrnFormatProvider();
        static string urnBase64UrlToken = "dXJuOnRlbmFudDphcm5hbWU6YWJjMTIzKCkrLC0uOj1AOyRfISonJTk5YQ2";
        static bool result;
    }
}
