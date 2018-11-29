using System;

namespace Elders.Cronus
{
    /// <summary>
    /// <see cref="UberUrnFormatProvider"/> always formats using <see cref="Base64UrlTokenUrnFormatProvider"/>. The real value
    /// is in the <see cref="Parse(string)"/> which tries to several <see cref="IUrnFormatProvider"/> in the following order:
    /// <see cref="Base64UrlTokenUrnFormatProvider"/> => <see cref="Base64UrnFormatProvider"/> => <see cref="PlainUrnFormatProvider"/>
    /// </summary>
    public class UberUrnFormatProvider : IUrnFormatProvider
    {
        private readonly Base64UrlTokenUrnFormatProvider base64UrlTokenUrnFormatProvider;
        private readonly Base64UrnFormatProvider base64UrnFormatProvider;

        public UberUrnFormatProvider()
        {
            base64UrlTokenUrnFormatProvider = new Base64UrlTokenUrnFormatProvider();
            base64UrnFormatProvider = new Base64UrnFormatProvider();
        }

        public string Format(IUrn urn)
        {
            if (urn is null) throw new ArgumentNullException(nameof(urn));

            return urn.Value;
        }

        public string Parse(string input)
        {
            if (base64UrlTokenUrnFormatProvider.CanParse(input))
                return base64UrlTokenUrnFormatProvider.Parse(input);

            if (base64UrnFormatProvider.CanParse(input))
                return base64UrnFormatProvider.Parse(input);

            return Parse(input);
        }
    }
}
