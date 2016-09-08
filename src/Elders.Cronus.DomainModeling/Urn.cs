using System;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus.DomainModeling
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
            NID = nid.ToLowerInvariant();
            NSS = nss.ToLowerInvariant();
            Value = Prefix + Delimiter + NID + Delimiter + NSS;
        }

        public string NID { get; private set; }

        public string NSS { get; private set; }

        public string Value { get; private set; }

        public IList<string> Parts
        {
            get
            {
                return Value.Split(new[] { DelimiterChar });
            }
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Urn urn)
        {
            return urn.Value;
        }

        static string urnRegex = @"\b(?<prefix>[urnURN]{3}):(?<nid>[a-zA-Z0-9][a-zA-Z0-9-]{0,31}):(?<nss>[a-zA-Z0-9()+,\-.=@;$_!:*'%\/?#]*[a-zA-Z0-9+=@$\/])";
        public static bool IsUrn(string candidate)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(candidate, urnRegex, System.Text.RegularExpressions.RegexOptions.None);
        }

        public static IUrn Parse(string urn)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn, urnRegex, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
                return new Urn(match.Groups["nid"].Value, match.Groups["nss"].Value);

            throw new ArgumentException("Invalid Urn. Expected: urn:nid:nss", nameof(urn));
        }
    }
}
