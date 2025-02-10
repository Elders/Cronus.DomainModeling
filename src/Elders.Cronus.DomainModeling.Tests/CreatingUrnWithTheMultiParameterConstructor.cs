namespace Elders.Cronus;

public class CreatingUrnWithTheMultiParameterConstructor
{
    static Exception exception;

    [Test]
    public async Task TheInputIsNotEncoded()
    {
        exception = Assert.Throws<ArgumentException>(() => new Urn("tenant", "something³"));

        await Assert.That(exception.Message).IsEqualTo("NSS is not valid something³ (Parameter 'nss')");
    }

    [Test]
    public async Task TheInputIsEncoded()
    {
        Urn urn = new Urn("tenant", "something%C2%B3");

        await Assert.That(urn.Value).IsEqualTo("urn:tenant:something%c2%b3");
    }

    [Test]
    public async Task TheInputIsNotValidEcoded()
    {
        exception = Assert.Throws<FormatException>(() => new Urn("tenant", "something%Ct%B3"));

        await Assert.That(exception.Message).IsEqualTo("Invalid URN format.");
    }
}
