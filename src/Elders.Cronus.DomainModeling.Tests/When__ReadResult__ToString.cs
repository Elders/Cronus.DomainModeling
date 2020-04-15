using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("ReadResult")]
    public class When__ReadResult__ToString
    {
        Because of = () => result.ToString();

        It should_have_good_string_representation = () => result.ToString().ShouldStartWith("ReadResult<String> => IsSuccess:");

        static ReadResult<string> result;
    }
}
