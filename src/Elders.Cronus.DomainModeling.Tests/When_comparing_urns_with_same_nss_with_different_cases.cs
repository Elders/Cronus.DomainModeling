using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_same_nss_with_different_cases
    {
        It should_not_be_equal = () => firstUrn.ShouldNotEqual(secondUrn);

        It should_not_have_equal_hashcodes = () => firstUrn.GetHashCode().ShouldNotEqual(secondUrn.GetHashCode());

        static IUrn firstUrn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'%99a");
        static IUrn secondUrn = new Urn("tenant", @"ArName:abc123()+,-.:=@;$_!*'%99a");
    }
}
