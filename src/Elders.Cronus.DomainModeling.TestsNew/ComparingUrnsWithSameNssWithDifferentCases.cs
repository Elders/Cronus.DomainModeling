namespace Elders.Cronus;

[ParallelGroup("TestsWithStaticConfigurations")]
public class ComparingUrnsWithSameNssWithDifferentCases
{
    static Urn firstUrn;
    static Urn secondUrn;

    [Before(Class)]
    public static void Setup()
    {
        Urn.UseCaseSensitiveUrns = true;

        firstUrn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'%99a");
        secondUrn = new Urn("tenant", @"ArName:abc123()+,-.:=@;$_!*'%99a");
    }

    [After(Class)]
    public static void Cleanup()
    {
        Urn.UseCaseSensitiveUrns = false;
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
