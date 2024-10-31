using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_equal_urns_using_equals_operator
    {
        It should_be_true = () => (firstUrn == secondUrn).ShouldBeTrue();

        static Urn firstUrn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'99a");
        static Urn secondUrn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'99a");
    }
}
