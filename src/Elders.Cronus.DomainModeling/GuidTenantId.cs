using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "07fb0a41-d004-4d90-a925-112cc5b1f0f5")]
    public class GuidTenantId : GuidId
    {
        [DataMember(Order = 2)]
        public string Tenant { get; set; }

        protected GuidTenantId() { }

        public GuidTenantId(Guid idBase, string aggregateRootName, string tenant) : base(idBase, aggregateRootName)
        {
            if (string.IsNullOrEmpty(tenant)) throw new ArgumentException("tenant is required.", nameof(tenant));
            Tenant = tenant;
        }

        public GuidTenantId(GuidTenantId idBase, string aggregateRootName) : base(idBase.Id, aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Default guid value is not allowed.", nameof(idBase));
            Tenant = idBase.Tenant;
        }

        public static bool IsValid(GuidTenantId aggregateRootId)
        {
            return GuidId.IsValid(aggregateRootId) && !string.IsNullOrEmpty(aggregateRootId.Tenant);
        }

        public override IUrn Urn { get { return new Urn(Tenant, AggregateRootName + ":" + Id.ToString()); } }
    }
}