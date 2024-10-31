using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_same_nids_and_nss_with_different_part_after_the_slash
    {
        It should_not_be_equal = () => firstUrn.ShouldNotEqual(secondUrn);

        It should_not_have_equal_hashcodes = () => firstUrn.GetHashCode().ShouldNotEqual(secondUrn.GetHashCode());

        static Urn firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'99a/abc");
        static Urn secondUrn = new Urn("tenant", @"arName:abc123()+,-.:=@;$_!*'99a/dfg");
    }
}
