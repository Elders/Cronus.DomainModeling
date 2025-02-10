namespace Elders.Cronus;

public class CreatingUrnWithSingleParameterConstructor
{
    static Exception exception;

    [Test]
    public async Task TheInputIsNotEncoded()
    {
        exception = Assert.Throws<ArgumentException>(() => new Urn("urn:tenant:something³"));

        await Assert.That(exception.Message).IsEqualTo("String is not a valid URN! (Parameter 'urnSpan')");
    }

    [Test]
    public async Task TheInputIsEncoded()
    {
        Urn urn = new Urn("urn:tenant:something%C2%B3");

        await Assert.That(urn.Value).IsEqualTo("urn:tenant:something%c2%b3");
    }

    [Test]
    public async Task TheInputIsNotValidEcoded()
    {
        exception = Assert.Throws<ArgumentException>(() => new Urn("urn:tenant:something%Ct%B3"));

        await Assert.That(exception.Message).IsEqualTo("String is not a valid URN! (Parameter 'urnSpan')");
    }
}
