using System;

namespace Elders.Cronus
{
    public class StringTenantUrn : Urn
    {
        const string regex = @"\b(?<prefix>[urnURN]{3}):(?<tenant>[a-zA-Z0-9][a-zA-Z0-9-]{0,31}):(?<arname>[a-zA-Z][a-zA-Z_\-.]{0,100}):?(?<id>[a-zA-Z0-9()+,\-.=@;$_!:*'%\/?#]*[a-zA-Z0-9+=@$\/])";

        public StringTenantUrn(string tenant, string arName, string id)
            : base(tenant, arName + Delimiter + id)
        {
            Tenant = tenant.ToLower();
            ArName = arName.ToLower();
            Id = id.ToLower();
        }

        public string Tenant { get; private set; }

        public string ArName { get; private set; }

        public string Id { get; private set; }

        public static bool TryParse(string urn, out StringTenantUrn parsedUrn)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn, regex, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
            {
                parsedUrn = new StringTenantUrn(match.Groups["tenant"].Value, match.Groups["arname"].Value, match.Groups["id"].Value);
                return true;
            }

            parsedUrn = null;

            return false;
        }

        new public static StringTenantUrn Parse(string urn)
        {
            var match = System.Text.RegularExpressions.Regex.Match(urn, regex, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
                return new StringTenantUrn(match.Groups["tenant"].Value, match.Groups["arname"].Value, match.Groups["id"].Value);

            throw new ArgumentException($"Invalid StringTenantUrn: {urn}", nameof(urn));
        }

        new public static StringTenantUrn Parse(string urn, IUrnFormatProvider proviver)
        {
            string plain = proviver.Parse(urn);
            return Parse(plain);
        }
    }
}
