using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("ReadResult")]
    public class When__ReadResult__is_successful
    {
        Because of = () => result = new ReadResult<int>(123);

        It should_have_positive_IsSuccess__state = () => result.IsSuccess.ShouldBeTrue();

        It should_have_data = () => result.Data.ShouldEqual(123);

        It should_have_negative__NotFound__state = () => result.NotFound.ShouldBeFalse();

        It should_have_negative__HasError__state = () => result.HasError.ShouldBeFalse();

        static ReadResult<int> result;
    }
}
