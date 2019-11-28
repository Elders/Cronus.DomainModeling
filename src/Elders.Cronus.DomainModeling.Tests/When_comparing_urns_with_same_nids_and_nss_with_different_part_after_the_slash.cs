using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_same_nids_and_nss_with_different_part_after_the_slash
    {
        It should_not_be_equal = () => firstUrn.ShouldNotEqual(secondUrn);

        static IUrn firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a/abc");
        static IUrn secondUrn = new Urn("tenant", @"arName:abc123()+,-.:=@;$_!*'%99a/dfg");
    }
}
