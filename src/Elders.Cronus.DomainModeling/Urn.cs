using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Elders.Cronus
{
    /// <summary>
    /// Uniform Resource Names (URNs) are intended to serve as persistent, location-independent, resource identifiers and are designed to make
    /// it easy to map other namespaces (which share the properties of URNs) into URN-space.Therefore, the URN syntax provides a means to encode
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
        /// The r-component is intended for passing parameters to URN resolution
        /// services and interpreted by those
        /// services.<see cref="https://tools.ietf.org/html/rfc8141#section-2.3.1"/>
        /// </summary>
        string R_Component { get; }

        /// <summary>
        /// The q-component is intended for passing parameters to either the
        /// named resource or a system that can supply the requested service, for
        ///  interpretation by that resource or system.
        /// <see cref="https://tools.ietf.org/html/rfc8141#section-2.3.2"/>
        /// </summary>
        string Q_Component { get; }

        /// <summary>
        /// The f-component is intended to be interpreted by the client as a
        //  specification for a location within, or region of, the named
        //  resource
        /// <see cref="https://tools.ietf.org/html/rfc8141#section-2.3.3"/>
        /// </summary>
        string F_Component { get; }
    }

    public static class UrnRegex
    {
        public static string Value => @"\A(?i:urn:(?!urn:)(?<nid>[a-z0-9][a-z0-9-]{1,31}[^-]):(?<nss>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)(?:\?\+(?<rcomponent>.*?))?(?:\?=(?<qcomponent>.*?))?(?:#(?<fcomponent>.*?))?)\z";

        public class Group
        {
            private string groupName;

            public Group(string groupName)
            {
                if (string.IsNullOrEmpty(groupName))
                    throw new ArgumentException(nameof(groupName));

                this.groupName = groupName;
            }

            public static Group NID => new Group("nid");
            public static Group NSS => new Group("nss");
            public static Group R_Component => new Group("rcomponent");
            public static Group Q_Component => new Group("qcomponent");
            public static Group F_Component => new Group("fcomponent");

            public override string ToString()
            {
                return groupName;
            }

            public static Group Create(string value)
            {
                switch (value.ToLowerInvariant())
                {
                    case "nid":
                        return NID;
                    case "nss":
                        return NSS;
                    case "rcomponent":
                        return R_Component;
                    case "qcomponent":
                        return Q_Component;
                    case "fcomponent":
                        return F_Component;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public static bool Matches(Uri urn)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn.AbsoluteUri, UrnRegex.Value, System.Text.RegularExpressions.RegexOptions.None);

            return match.Success;
        }

        public static string GetGroup(string urnString, Group group)
        {
            var urn = new Uri(urnString);
            return GetGroup(urn, group);
        }

        public static string GetGroup(Uri urn, Group group)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn.AbsoluteUri, UrnRegex.Value, System.Text.RegularExpressions.RegexOptions.None);
            return match.Groups[group.ToString()].Value;
        }
    }

    public class Urn : IUrn, IEquatable<Urn>
    {
        public static char PARTS_DELIMITER = ':';
        public static char HIERARCHICAL_DELIMITER = '/';
        public const string UriSchemeUrn = "urn";

        private readonly Uri uri;

        protected Urn() { }

        public Urn(string urnString)
        {
            if (IsUrn(urnString) == false)
                throw new ArgumentException("String is not a URN!");

            this.uri = new Uri(urnString);
        }

        /// <summary>
        /// Initializes a new URN
        /// </summary>
        /// <param name="nid">The Namespace Identifier</param>
        /// <param name="nss">The Namespace Specific String</param>
        public Urn(string nid, string nss, string rcomponent = null, string qcomponent = null, string fcomponent = null)
        {
            if (string.IsNullOrEmpty(nid))
                throw new ArgumentException("NID is not valid", nameof(nid));

            if (string.IsNullOrEmpty(nss))
                throw new ArgumentException("NSS is not valid", nameof(nss));

            var urn = new StringBuilder($"urn{PARTS_DELIMITER}{nid}{PARTS_DELIMITER}{nss}");

            if (string.IsNullOrEmpty(rcomponent) == false)
                urn.Append($"?+{rcomponent}");

            if (string.IsNullOrEmpty(qcomponent) == false)
                urn.Append($"?={qcomponent}");

            if (string.IsNullOrEmpty(fcomponent) == false)
                urn.Append($"#{fcomponent}");


            this.uri = new Uri(urn.ToString());
        }

        public string NID => UrnRegex.GetGroup(uri, UrnRegex.Group.NID);

        public string NSS => UrnRegex.GetGroup(uri, UrnRegex.Group.NSS);

        public string Value => uri.ToString();

        public string R_Component => UrnRegex.GetGroup(uri, UrnRegex.Group.R_Component);

        public string Q_Component => UrnRegex.GetGroup(uri, UrnRegex.Group.Q_Component);

        public string F_Component => UrnRegex.GetGroup(uri, UrnRegex.Group.F_Component);

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

        public static bool IsUrn(string candidate)
        {
            var uri = new Uri(candidate);
            if (uri.Scheme.Equals(UriSchemeUrn) == false)
                return false;

            return UrnRegex.Matches(uri);
        }

        public static bool IsUrn(string candidate, IUrnFormatProvider provider)
        {
            try
            {
                var parsedUrn = Parse(candidate, provider);
                return IsUrn(parsedUrn);
            }
            catch (Exception)
            {
                // This method must not throw exceptions
                return false;
            }
        }

        public static Urn Parse(string urn)
        {
            return new Urn(urn);
        }

        public static Urn Parse(string urn, IUrnFormatProvider proviver)
        {
            string plain = proviver.Parse(urn);
            return Parse(plain);
        }

        public override bool Equals(object comparand)
        {
            if (comparand is Urn)
                return this.Equals(comparand as Urn);

            return base.Equals(comparand);
        }

        /// <summary>
        /// Scheme and NID are case insensitive, NSS is case sensitive (except for the percent-encoded transformation where needed)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Urn other)
        {
            return $"{NID.ToLower()}:{NSS}".Equals($"{other.NID.ToLower()}:{other.NSS}");
        }
    }
}
