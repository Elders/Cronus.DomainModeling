using System;
using System.Runtime.Serialization;
using System.Text;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "08fe27ca-411e-45ce-94ce-5d64c45eae6c")]
    public class StringId : AggregateRootId
    {
        [DataMember(Order = 1)]
        public string Id { get; private set; }

        protected StringId() { }

        public StringId(string idBase, string aggregateRootName) : base(aggregateRootName)
        {
            if (string.IsNullOrWhiteSpace(idBase)) throw new ArgumentException("Empty string value is not allowed.", nameof(idBase));
            Id = idBase;
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@"), Encoding.UTF8.GetBytes(Id));
        }

        public StringId(StringId idBase, string aggregateRootName) : base(aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Empty string value is not allowed.", nameof(idBase));
            Id = idBase.Id;
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@"), Encoding.UTF8.GetBytes(Id));
        }

        public static bool IsValid(StringId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && string.IsNullOrWhiteSpace(aggregateRootId.Id) == false;
        }

        public override string ToString()
        {
            return AggregateRootName + "@" + Id.ToString() + "||" + base.ToString();
        }
    }

    [DataContract(Name = "b78e63f3-1443-4e82-ba4c-9b12883518b9")]
    public class StringTenantId : StringId
    {
        [DataMember(Order = 2)]
        public string Tenant { get; set; }

        protected StringTenantId() { }

        public StringTenantId(string idBase, string aggregateRootName, string tenant) : base(idBase, aggregateRootName)
        {
            if (string.IsNullOrEmpty(tenant)) throw new ArgumentException("tenant is required.", nameof(tenant));
            Tenant = tenant;
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@" + tenant + "@"), Encoding.UTF8.GetBytes(Id));
        }

        public StringTenantId(StringTenantId idBase, string aggregateRootName) : base(idBase.Id, aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Invalid base.", nameof(idBase));
            Tenant = idBase.Tenant;
            base.RawId = ByteArrayHelper.Combine(Encoding.UTF8.GetBytes(AggregateRootName + "@" + Tenant + "@"), Encoding.UTF8.GetBytes(Id));
        }

        public static bool IsValid(StringTenantId aggregateRootId)
        {
            return StringId.IsValid(aggregateRootId) && !string.IsNullOrEmpty(aggregateRootId.Tenant);
        }

        public override string ToString()
        {
            return AggregateRootName + "@" + Tenant + "@" + Id.ToString() + "||" + base.ToString();
        }
    }
}