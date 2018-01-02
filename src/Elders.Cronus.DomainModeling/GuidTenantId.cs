using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Elders.Cronus
{
    [DataContract(Name = "07fb0a41-d004-4d90-a925-112cc5b1f0f5")]
    public class GuidTenantId : AggregateRootId
    {
        [DataMember(Order = 1)]
        public Guid Id { get; private set; }

        [DataMember(Order = 2)]
        public string Tenant { get; private set; }

        protected GuidTenantId() { }

        public GuidTenantId(Guid idBase, string aggregateRootName, string tenant) : base(aggregateRootName)
        {
            if (string.IsNullOrEmpty(tenant)) throw new ArgumentException("tenant is required.", nameof(tenant));
            Tenant = tenant;
            Id = idBase;
            RawId = setRawId(Urn);
        }

        public GuidTenantId(GuidTenantId idBase, string aggregateRootName) : base(aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Invalid base.", nameof(idBase));
            Id = idBase.Id;
            Tenant = idBase.Tenant;
            RawId = setRawId(Urn);
        }

        public GuidTenantId(IUrn urn, string aggregateRootName)
        {
            if (ReferenceEquals(null, urn)) throw new ArgumentNullException(nameof(urn));

            var tenantUrn = new Urn(urn);
            Tenant = tenantUrn.NID;
            AggregateRootName = tenantUrn.Parts[2].ToLowerInvariant();
            if (AggregateRootName.Equals(aggregateRootName.ToLowerInvariant()) == false)
            {
                throw new ArgumentException("Invalid Urn for " + aggregateRootName + " AggregateRootId");
            }
            Id = Guid.Parse(string.Join(":", tenantUrn.Parts.Skip(3).ToString()));
            RawId = setRawId(Urn);
        }

        public static bool IsValid(GuidTenantId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && aggregateRootId.Id != default(Guid) && !string.IsNullOrEmpty(aggregateRootId.Tenant);
        }

        public override IUrn Urn { get { return new Urn(Tenant, AggregateRootName + ":" + Id); } }
    }
}
