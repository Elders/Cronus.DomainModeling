using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_same_nids_with_different_cases
    {
        It should_be_equal = () => firstUrn.ShouldEqual(secondUrn);

        It should_have_equal_hashcodes = () => firstUrn.GetHashCode().ShouldEqual(secondUrn.GetHashCode());

        static Urn firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'99a");
        static Urn secondUrn = new Urn("tenant", @"arName:abc123()+,-.:=@;$_!*'99a");
    }
}
