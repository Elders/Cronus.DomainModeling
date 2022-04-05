using System;

namespace Elders.Cronus;

public class PlainUrnFormatProvider : IUrnFormatProvider
{
    public string Format(IUrn urn)
    {
        if (urn is null) throw new ArgumentNullException(nameof(urn));

        return urn.Value;
    }

    public string Parse(string input)
    {
        if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));

        return input;
    }
}
