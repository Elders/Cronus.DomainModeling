using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_parsing_string_to_urn_using__PlainUrnFormatProvider__
    {
        Because of = () => result = Urn.Parse(urnPlain, provider);

        It should_build_urn = () => result.Value.ShouldEqual(urn.Value);

        static PlainUrnFormatProvider provider = new PlainUrnFormatProvider();
        static Urn urn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static string urnPlain = @"urn:Tenant:arName:abc123()+,-.:=@;$_!*'%99a";
        static Urn result;
    }

    [Subject("Urn")]
    public class When_matching_string_to_URN_regex_with_full_length_NID
    {
        Because of = () => result = Urn.IsUrn(fullSizeUrn);

        It should_succeed = () => result.ShouldBeTrue();

        static string fullSizeUrn = @"urn:X123456789012345678901234567890X:arName:abc123()+,-.:=@;$_!*'%99a";
        static bool result = false;
    }

    [Subject("Urn")]
    public class When_matching_string_to_URN_regex_with_min_length_NID
    {
        Because of = () => result = Urn.IsUrn(urn);

        It should_succeed = () => result.ShouldBeTrue();

        static string urn = @"urn:XX:arName:abc123()+,-.:=@;$_!*'%99a";
        static bool result = false;
    }

    [Subject("Urn")]
    public class When_matching_string_to_URN_regex_invalid_NID
    {
        Because of = () => result = Urn.IsUrn(fullSizeUrn);

        It should_succeed = () => result.ShouldBeFalse();

        static string fullSizeUrn = @"urn:X345678901234567890X-:arName:abc123()+,-.:=@;$_!*'%99a";
        static bool result = false;
    }

    [Subject("Urn")]
    public class When_matching_string_to_URN_regex_with_NID_exceeding_the_max_length
    {
        Because of = () => result = Urn.IsUrn(fullSizeUrn);

        It should_succeed = () => result.ShouldBeFalse();

        static string fullSizeUrn = @"urn:X123456789012345678901234567890X1:arName:abc123()+,-.:=@;$_!*'%99a";
        static bool result = false;
    }
}
