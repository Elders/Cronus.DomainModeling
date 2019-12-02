using System;

namespace Elders.Cronus
{
    public class StringTenantUrn : Urn
    {
        const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<id>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";

        public StringTenantUrn(string tenant, string arName, string id)
            : base(tenant, $"{arName}{PARTS_DELIMITER}{id}".ToLower())
        {
            Id = id.ToLower();
            Tenant = tenant.ToLower();
            ArName = arName.ToLower();
        }

        public string Tenant { get; private set; }

        public string ArName { get; private set; }

        public string Id { get; private set; }

        public static bool TryParse(string urn, out StringTenantUrn parsedUrn)
        {
            parsedUrn = null;

            if (IsUrn(urn) == false)
                return false;

            Urn baseUrn = new Urn(urn);

            var match = System.Text.RegularExpressions.Regex.Match(baseUrn.NSS, NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
            {
                parsedUrn = new StringTenantUrn(baseUrn.NID, match.Groups["arname"].Value, match.Groups["id"].Value);
                return true;
            }

            return false;
        }

        new public static StringTenantUrn Parse(string urn)
        {
            Urn baseUrn = new Urn(urn);

            var match = System.Text.RegularExpressions.Regex.Match(baseUrn.NSS, NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
            {
                return new StringTenantUrn(baseUrn.NID, match.Groups["arname"].Value, match.Groups["id"].Value);
            }

            throw new ArgumentException($"Invalid StringTenantUrn: {urn}", nameof(urn));
        }

        new public static StringTenantUrn Parse(string urn, IUrnFormatProvider proviver)
        {
            string plain = proviver.Parse(urn);
            return Parse(plain);
        }
    }
}
