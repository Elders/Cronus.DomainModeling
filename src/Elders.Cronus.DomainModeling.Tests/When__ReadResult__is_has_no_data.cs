﻿using Machine.Specifications;

namespace Elders.Cronus
{
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
}
