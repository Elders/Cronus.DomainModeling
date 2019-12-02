﻿using System;

namespace Elders.Cronus
{
    public class StringTenantEntityUrn : Urn
    {
        const string NSS_REGEX = @"\A(?i:(?<arName>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<arId>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)\/(?<entityName>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+)\/(?<entityId>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";

        public StringTenantEntityUrn(StringTenantUrn arUrn, string entityName, string id)
            : base(arUrn.Tenant, $"{arUrn.NSS}{HIERARCHICAL_DELIMITER}{entityName}{HIERARCHICAL_DELIMITER}{id}".ToLower())
        {
            Id = id.ToLower();
            AggregateUrn = arUrn ?? throw new ArgumentNullException(nameof(arUrn));
            EntityName = entityName.ToLower();
        }

        public StringTenantUrn AggregateUrn { get; private set; }

        public string EntityName { get; private set; }

        public string Id { get; private set; }

        new public static StringTenantEntityUrn Parse(string urn)
        {
            Urn baseUrn = new Urn(urn);

            var match = System.Text.RegularExpressions.Regex.Match(baseUrn.NSS, NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
            {
                var rootUrn = new StringTenantUrn(baseUrn.NID, match.Groups["arname"].Value, match.Groups["arId"].Value);
                return new StringTenantEntityUrn(rootUrn, match.Groups["entityName"].Value, match.Groups["entityId"].Value);
            }

            throw new ArgumentException($"Invalid StringTenantUrn: {urn}", nameof(urn));
        }

        new public static StringTenantEntityUrn Parse(string urn, IUrnFormatProvider proviver)
        {
            string plain = proviver.Parse(urn);
            return Parse(plain);
        }
    }
}
