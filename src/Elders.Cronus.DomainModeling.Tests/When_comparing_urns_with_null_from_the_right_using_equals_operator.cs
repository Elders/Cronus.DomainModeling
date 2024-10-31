using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_null_from_the_right_using_equals_operator
    {
        It should_be_false = () => (urn == null).ShouldBeFalse();

        It should_be_true = () => (nullUrn == null).ShouldBeTrue();

        static Urn nullUrn = null;
        static Urn urn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'99a");
    }
}
