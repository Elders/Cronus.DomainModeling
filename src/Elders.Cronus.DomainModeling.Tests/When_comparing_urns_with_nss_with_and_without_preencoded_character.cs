using Machine.Specifications;

namespace Elders.Cronus
{
    [Subject("Urn")]
    public class When_comparing_urns_with_nss_with_and_without_preencoded_character
    {
        It should_not_be_equal = () => firstUrn.ShouldNotEqual(secondUrn);

        It should_not_have_equal_hashcodes = () => firstUrn.GetHashCode().ShouldNotEqual(secondUrn.GetHashCode());

        static IUrn firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        static IUrn secondUrn = new Urn("tenant", @"arName:abc123()+%2c-.:=@;$_!*'%99a");
    }
}
