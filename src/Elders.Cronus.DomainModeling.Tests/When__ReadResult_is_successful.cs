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

    [Subject("ReadResult")]
    public class When__ReadResult__is_has_no_data
    {
        Because of = () => result = new ReadResult<object>();

        It should_have_positive_IsSuccess__state = () => result.IsSuccess.ShouldBeFalse();

        It should_have_data = () => result.Data.ShouldBeNull();

        It should_have_positive__NotFound__state = () => result.NotFound.ShouldBeTrue();

        It should_have_negative__HasError__state = () => result.HasError.ShouldBeFalse();

        static ReadResult<object> result;
    }

    [Subject("ReadResult")]
    public class When__ReadResult__is_has_no_data_and_has_a_hint
    {
        Because of = () => result = ReadResult<object>.WithNotFoundHint("Unable to find record with id=123");

        It should_have_positive_IsSuccess__state = () => result.IsSuccess.ShouldBeFalse();

        It should_have_data = () => result.Data.ShouldBeNull();

        It should_have_positive__NotFound__state = () => result.NotFound.ShouldBeTrue();

        It should_have_negative__HasError__state = () => result.HasError.ShouldBeFalse();

        static ReadResult<object> result;
    }

    [Subject("ReadResult")]
    public class When__ReadResult__has_error
    {
        Because of = () => result = ReadResult<string>.WithError("error");

        It should_have_positive_IsSuccess__state = () => result.IsSuccess.ShouldBeFalse();

        It should_have_data = () => result.Data.ShouldBeNull();

        It should_have_positive__NotFound__state = () => result.NotFound.ShouldBeTrue();

        It should_have_negative__HasError__state = () => result.HasError.ShouldBeTrue();

        static ReadResult<string> result;
    }

    [Subject("ReadResult")]
    public class When__ReadResult__ToString
    {
        Because of = () => result.ToString();

        It should_have_good_string_representation = () => result.ToString().ShouldStartWith("ReadResult<String> => IsSuccess:");

        static ReadResult<string> result;
    }

    [Subject("ReadResult")]
    public class When__ReadResult__has_exception
    {
        Because of = () => result = ReadResult<string>.WithError(new System.Exception("exception"));

        It should_have_positive_IsSuccess__state = () => result.IsSuccess.ShouldBeFalse();

        It should_have_data = () => result.Data.ShouldBeNull();

        It should_have_positive__NotFound__state = () => result.NotFound.ShouldBeTrue();

        It should_have_negative__HasError__state = () => result.HasError.ShouldBeTrue();

        It should_have_good_string_representation = () => result.ToString().ShouldStartWith("ReadResult<String> => IsSuccess:");

        static ReadResult<string> result;
    }

    [Subject("ReadResult")]
    public class When__ReadResult__is_not_initialized
    {
        Because of = () => result.ToString();

        It should_have_positive_IsSuccess__state = () => result.IsSuccess.ShouldBeFalse();

        It should_have_data = () => result.Data.ShouldBeNull();

        It should_have_positive__NotFound__state = () => result.NotFound.ShouldBeTrue();

        It should_have_negative__HasError__state = () => result.HasError.ShouldBeFalse();

        static ReadResult<string> result;
    }
}
