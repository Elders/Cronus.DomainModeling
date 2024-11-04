using System;

namespace Elders.Cronus;

public class ComparingUrnsWithNullFromTheRightUsingEqualsOperator
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
        await Assert.That(urn == null).IsFalse();
    }

    [Test]
    public async Task ShouldReturnTrue()
    {
        await Assert.That(nullUrn == null).IsTrue();
    }
}
