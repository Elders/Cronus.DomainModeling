using System;
using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_invalid_string_to_urn_using__UberUrlTokenUrnFormatProvider__
    {
        Because of = () => exception = Catch.Exception(() => Urn.Parse(randomString, provider));

        It should_throw_exception = () => exception.ShouldNotBeNull();

        static Exception exception;
        static UberUrnFormatProvider provider = new UberUrnFormatProvider();
        static string randomString = "670";
    }
}
