namespace Elders.Cronus;

public class ComparingUrnsWithNssWithAndWithoutPreencodedCharacter
{
    static Urn firstUrn;
    static Urn secondUrn;

    [Before(Class)]
    public static void Setup()
    {
        firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a");
        secondUrn = new Urn("tenant", @"arName:abc123()+%2c-.:=@;$_!*'%99a");
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
