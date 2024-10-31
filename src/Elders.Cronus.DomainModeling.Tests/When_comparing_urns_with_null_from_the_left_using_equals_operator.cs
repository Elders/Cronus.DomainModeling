using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_null_from_the_left_using_equals_operator
    {
        It should_be_false = () => (null == urn).ShouldBeFalse();

        It should_be_true = () => (null == nullUrn).ShouldBeTrue();

        static Urn nullUrn = null;
        static Urn urn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'99a");
    }
}
