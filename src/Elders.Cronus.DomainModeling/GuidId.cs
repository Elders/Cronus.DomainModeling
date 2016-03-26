using System;
using System.Runtime.Serialization;
using System.Text;

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
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@"), Id.ToByteArray());
        }

        public GuidId(GuidId idBase, string aggregateRootName) : base(aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Default guid value is not allowed.", nameof(idBase));
            Id = idBase.Id;
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@"), Id.ToByteArray());
        }

        public static bool IsValid(GuidId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && aggregateRootId.Id != default(Guid);
        }

        public override string ToString()
        {
            return AggregateRootName + "@" + Id.ToString() + "||" + base.ToString();
        }
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
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@" + tenant + "@"), Id.ToByteArray());
        }

        public GuidTenantId(GuidTenantId idBase, string aggregateRootName) : base(idBase.Id, aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Default guid value is not allowed.", nameof(idBase));
            Tenant = idBase.Tenant;
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@" + Tenant + "@"), Id.ToByteArray());
        }

        public static bool IsValid(GuidTenantId aggregateRootId)
        {
            return GuidId.IsValid(aggregateRootId) && !string.IsNullOrEmpty(aggregateRootId.Tenant);
        }

        public override string ToString()
        {
            return AggregateRootName + "@" + Tenant + "@" + Id.ToString() + "||" + base.ToString();
        }
    }
}