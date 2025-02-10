namespace Elders.Cronus;

public class ComparingUrnsWithNullFromTheRightUsingEqualsOperator
{
    static Urn urn;
    static Urn nullUrn;

    [Before(Class)]
    public static void Setup()
    {
        urn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'%99a");
        nullUrn = null;
    }

    [Test]
    public async Task ShouldReturnFalse()
    {
        await Assert.That(urn == null).IsFalse();
    }

    [Test]
    public async Task ShouldReturnTrue()
    {
        await Assert.That(nullUrn == null).IsTrue();
    }
}
