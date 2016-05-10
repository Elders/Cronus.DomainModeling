using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "b3b879e8-b11b-4a57-8f95-0c1c7512fd73")]
    public class GuidId : AggregateRootId
    {
        [DataMember(Order = 1)]
        public Guid Id { get; private set; }

        protected GuidId() { }

        public GuidId(Guid idBase, string aggregateRootName) : base(aggregateRootName)
        {
            if (idBase == default(Guid)) throw new ArgumentException("Default guid value is not allowed.", nameof(idBase));
            Id = idBase;
        }

        public GuidId(GuidId idBase, string aggregateRootName) : base(aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Default guid value is not allowed.", nameof(idBase));
            Id = idBase.Id;
        }

        public static bool IsValid(GuidId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && aggregateRootId.Id != default(Guid);
        }

        public override IUrn Urn { get { return new Urn(AggregateRootName + ":" + Id.ToString()); } }
    }

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

        public override IUrn Urn { get { return new TenantUrn(Tenant, AggregateRootName + ":" + Id.ToString()); } }
    }
}