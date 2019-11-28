using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_same_nids_and_nss_but_with_different_components
    {
        It should_be_equal = () => firstUrn.ShouldEqual(secondUrn);

        static IUrn firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a", rcomponent: "abc");
        static IUrn secondUrn = new Urn("tenant", @"arName:abc123()+,-.:=@;$_!*'%99a", qcomponent: "dfg");
    }
}
