namespace Elders.Cronus;

public class ComparingUrnsWithSameNidsWithDifferentCases
{
    static Urn firstUrn;
    static Urn secondUrn;

    [Before(Class)]
    public static void Setup()
    {
        firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        secondUrn = new Urn("tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
    }

    [Test]
    public async Task ShouldBeEqual()
    {
        await Assert.That(firstUrn).IsEqualTo(secondUrn);
    }

    [Test]
    public async Task ShouldHaveEqualHashcodes()
    {
        await Assert.That(firstUrn.GetHashCode()).IsEqualTo(secondUrn.GetHashCode());
    }
}
