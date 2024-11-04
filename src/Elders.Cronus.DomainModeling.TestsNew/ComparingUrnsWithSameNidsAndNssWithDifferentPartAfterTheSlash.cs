namespace Elders.Cronus;

public class ComparingUrnsWithSameNidsAndNssWithDifferentPartAfterTheSlash
{
    static Urn firstUrn;
    static Urn secondUrn;

    [Before(Class)]
    public static void Setup()
    {
        firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'99a/abc");
        secondUrn = new Urn("tenant", @"arName:abc123()+,-.:=@;$_!*'99a/dfg");
    }

    [Test]
    public async Task ShouldNotBeEqual()
    {
        await Assert.That(firstUrn).IsNotEqualTo(secondUrn);
    }

    [Test]
    public async Task ShouldNotHaveEqualHashcodes()
    {
        await Assert.That(firstUrn.GetHashCode()).IsNotEqualTo(secondUrn.GetHashCode());
    }
}
