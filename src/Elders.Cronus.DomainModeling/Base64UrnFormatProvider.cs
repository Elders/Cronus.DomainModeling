using System;

namespace Elders.Cronus
{
    public class Base64UrnFormatProvider : IUrnFormatProvider
    {
        public string Format(IUrn urn)
        {
            if (urn is null) throw new ArgumentNullException(nameof(urn));

            return urn.Value.Base64Encode();
        }

        public string Parse(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
            if (input.IsBase64String() == false) throw new ArgumentException($"Invalid base64 value `{input}`", nameof(input));

            return input.Base64Decode();
        }

        public bool CanParse(string input)
        {
            return input.IsBase64String();
        }
    }
}
