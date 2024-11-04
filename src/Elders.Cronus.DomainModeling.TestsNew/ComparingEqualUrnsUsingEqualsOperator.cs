namespace Elders.Cronus;

public class ComparingEqualUrnsUsingEqualsOperator
{
    static Urn firstUrn;
    static Urn secondUrn;

    [Before(Class)]
    public static void Startup()
    {
        firstUrn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'%99a");
        secondUrn = new Urn("tenant", @"arname:abc123()+,-.:=@;$_!*'%99a");
    }

    [Test]
    public async Task MustBeEqual()
    {
        await Assert.That(firstUrn == secondUrn).IsTrue();
    }
}
