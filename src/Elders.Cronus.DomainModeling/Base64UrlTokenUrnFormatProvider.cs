using System;

namespace Elders.Cronus
{
    public class Base64UrlTokenUrnFormatProvider : IUrnFormatProvider
    {
        public string Format(IUrn urn)
        {
            if (urn is null) throw new ArgumentNullException(nameof(urn));

            return urn.Value.Base64UrlTokenEncode();
        }

        public string Parse(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
            if (input.CanBase64UrlTokenDecode() == false) throw new ArgumentException($"Invalid base64 url token value `{input}`", nameof(input));

            return input.Base64UrlTokenDecode();
        }

        public bool CanParse(string input)
        {
            return input.CanBase64UrlTokenDecode();
        }
    }
}
