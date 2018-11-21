using System;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus
{
    /// <summary>
    /// Uniform Resource Names (URNs) are intended to serve as persistent, location-independent, resource identifiers and are designed to make
    /// it easy to map other namespaces(which share the properties of URNs) into URN-space.Therefore, the URN syntax provides a means to encode
    /// character data in a form that can be sent in existing protocols, transcribed on most keyboards, etc.
    /// </summary>
    /// <example>"urn:NID:NSS</example>
    public interface IUrn
    {
        /// <summary>
        /// Gets the Namespace Identifier. The Namespace ID determines the _syntactic_ interpretation of the Namespace Specific String
        /// </summary>
        string NID { get; }

        /// <summary>
        /// Gets the Namespace Specific String
        /// </summary>
        string NSS { get; }

        /// <summary>
        /// Gets the URN
        /// </summary>
        string Value { get; }

        /// <summary>
        /// Gets the URN parts
        /// </summary>
        IList<string> Parts { get; }
    }

    public class Urn : IUrn
    {
        public const string Prefix = "urn";

        public const char DelimiterChar = ':';

        public const string Delimiter = ":";

        protected Urn() { }

        /// <summary>
        /// Initializes a new URN
        /// </summary>
        /// <param name="nid">The Namespace Identifier</param>
        /// <param name="nss">The Namespace Specific String</param>
        public Urn(string nid, string nss)
        {
            Initialize(nid, nss);
        }

        public Urn(IUrn urn)
        {
            Initialize(urn.NID, urn.NSS);
        }

        protected void Initialize(string nid, string nss)
        {
            if (string.IsNullOrEmpty(nid)) throw new ArgumentNullException(nameof(nid));
            if (string.IsNullOrEmpty(nss)) throw new ArgumentNullException(nameof(nss));

            NID = nid.ToLower();
            NSS = nss.ToLower();
            Value = $"{Prefix}{Delimiter}{NID}{Delimiter}{NSS}";

            if (IsUrn(Value) == false)
                throw new ArgumentException($"Invalid urn `{Value}` built from parameters `nid` and `nss`");
        }

        public string NID { get; private set; }

        public string NSS { get; private set; }

        public string Value { get; private set; }

        public IList<string> Parts { get { return Value.Split(new[] { DelimiterChar }); } }

        public override string ToString()
        {
            return Value;
        }

        public string ToString(IUrnFormatProvider provider)
        {
            return provider.Format(this);
        }

        public static implicit operator string(Urn urn)
        {
            return urn.Value;
        }

        const string UrnRegex = @"\A(?i:urn:(?!urn:)(?<nid>[a-z0-9][a-z0-9-]{1,31}):(?<nss>(?:[a-z0-9()+,-.:=@;$_!*']|%[0-9a-f]{2})+))\z";
        public static bool IsUrn(string candidate)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(candidate, UrnRegex, System.Text.RegularExpressions.RegexOptions.None);
        }

        public static Urn Parse(string urn)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn, UrnRegex, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
                return new Urn(match.Groups["nid"].Value, match.Groups["nss"].Value);

            throw new ArgumentException("Invalid Urn. Expected: urn:nid:nss", nameof(urn));
        }

        public static Urn Parse(string urn, IUrnFormatProvider proviver)
        {
            string plain = proviver.Parse(urn);
            return Parse(plain);
        }
    }
}
