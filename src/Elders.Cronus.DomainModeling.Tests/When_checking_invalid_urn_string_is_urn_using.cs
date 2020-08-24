using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_checking_invalid_urn_string_is_urn_using
    {
        Because of = () => result = Urn.IsUrn("invalid");

        It should_be_true = () => result.ShouldBeFalse();

        static bool result;
    }
}
