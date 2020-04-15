using System;
using System.Runtime.Serialization;

namespace Elders.Cronus
{
    [DataContract(Name = "b78e63f3-1443-4e82-ba4c-9b12883518b9")]
    public class AggregateUrn : Urn, IAggregateRootId
    {
        const string NSS_REGEX = @"\A(?i:(?<arname>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+):(?<id>(?:[-a-z0-9()+,.:=@;$_!*'&~\/]|%[0-9a-f]{2})+))\z";

        protected AggregateUrn()
        {
            this.id = string.Empty;
            this.tenant = string.Empty;
            this.aggregateRootName = string.Empty;
        }

        public AggregateUrn(string tenant, string arName, string id)
            : base(tenant, $"{arName}{PARTS_DELIMITER}{id}")
        {
            this.id = id;
            this.tenant = tenant;
            this.aggregateRootName = arName;
        }

        public AggregateUrn(string tenant, IAggregateRootId urn)
            : base(tenant, $"{urn.AggregateRootName}{PARTS_DELIMITER}{urn.Id}")
        {
            this.id = urn.Id;
            this.tenant = tenant;
            this.aggregateRootName = urn.AggregateRootName;
        }

        string id;
        string tenant;
        string aggregateRootName;
        private bool isFullyInitialized;

        protected override void DoFullInitialization()
        {
            if (isFullyInitialized == false)
            {
                base.DoFullInitialization();

                var match = System.Text.RegularExpressions.Regex.Match(nss, NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);
                if (match.Success)
                {
                    id = match.Groups["id"].Value;
                    aggregateRootName = match.Groups["arname"].Value;
                    tenant = nid;
                }

                isFullyInitialized = true;
            }
        }

        public string Id { get { DoFullInitialization(); return id; } }

        public string Tenant { get { DoFullInitialization(); return tenant; } }

        public string AggregateRootName { get { DoFullInitialization(); return aggregateRootName; } }

        public static bool TryParse(string urn, out AggregateUrn parsedUrn)
        {
            parsedUrn = null;

            if (IsUrn(urn) == false)
                return false;

            Urn baseUrn = new Urn(urn);

            var match = System.Text.RegularExpressions.Regex.Match(baseUrn.NSS, NSS_REGEX, System.Text.RegularExpressions.RegexOptions.None);
            if (match.Success)
            {
                parsedUrn = new AggregateUrn(baseUrn.NID, match.Groups["arname"].Value, match.Groups["id"].Value);
                return true;
            }

            return false;
        }

        new public static AggregateUrn Parse(string urn)
        {
            if (TryParse(urn, out AggregateUrn parsedUrn))
            {
                return parsedUrn;
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(AggregateUrn)}: {urn}", nameof(urn));
            }
        }

        new public static AggregateUrn Parse(string urn, IUrnFormatProvider proviver)
        {
            string plain = proviver.Parse(urn);
            return Parse(plain);
        }
    }
}
