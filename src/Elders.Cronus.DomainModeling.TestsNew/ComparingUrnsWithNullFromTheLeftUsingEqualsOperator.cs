namespace Elders.Cronus;

public class ComparingUrnsWithNullFromTheLeftUsingEqualsOperator
{
    static Urn urn;
    static Urn nullUrn;

    [Before(Class)]
    public static void Setup()
    {
        urn = new Urn("urn:tenant:arname:123");
        nullUrn = null;
    }

    [Test]
    public async Task ShouldReturnFalse()
    {
        await Assert.That(null == urn).IsFalse();
    }

    [Test]
    public async Task ShouldReturnTrue()
    {
        await Assert.That(null == nullUrn).IsTrue();
    }
}

public class ComparingUrnsWithSameNidsAndNssWithDifferentPartAfterTheSlash
{
    static Urn firstUrn;
    static Urn secondUrn;

    [Before(Class)]
    public static void Setup()
    {
        firstUrn = new Urn("Tenant", @"arName:abc123()+,-.:=@;$_!*'%99a/abc");
        secondUrn = new Urn("tenant", @"arName:abc123()+,-.:=@;$_!*'%99a/dfg");
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
