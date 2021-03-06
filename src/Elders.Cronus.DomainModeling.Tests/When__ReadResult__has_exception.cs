﻿using Machine.Specifications;

namespace Elders.Cronus
{
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
}
