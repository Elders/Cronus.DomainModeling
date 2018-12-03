using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_checking_invalid_urn_string_is_urn_using__Base64UrlTokenUrnFormatProvider__
    {
        Because of = () => result = Urn.IsUrn(urnBase64UrlToken, provider);

        It should_be_false = () => result.ShouldBeFalse();

        static UberUrnFormatProvider provider = new UberUrnFormatProvider();
        static string urnBase64UrlToken = "InvalidUrn";
        static bool result;
    }
}
