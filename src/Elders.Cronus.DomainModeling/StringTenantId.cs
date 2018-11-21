using System;
using System.Runtime.Serialization;

namespace Elders.Cronus
{
    [DataContract(Name = "b78e63f3-1443-4e82-ba4c-9b12883518b9")]
    public class StringTenantId : AggregateRootId
    {
        [DataMember(Order = 1)]
        public string Id { get; private set; }

        [DataMember(Order = 2)]
        public string Tenant { get; private set; }

        protected StringTenantId() { }

        public StringTenantId(string idBase, string aggregateRootName, string tenant)
            : base(aggregateRootName, new Urn(tenant, $"{aggregateRootName}:{idBase}"))
        {
            Id = idBase;
            Tenant = tenant;
        }

        public StringTenantId(StringTenantId idBase, string aggregateRootName) : this(idBase.Id, aggregateRootName, idBase.Tenant) { }

        public StringTenantId(StringTenantUrn urn, string aggregateRootName) : base(urn.ArName, urn)
        {
            if (urn.ArName.Equals(aggregateRootName, StringComparison.OrdinalIgnoreCase) == false)
                throw new ArgumentException("AggregateRootName missmatch");

            Id = urn.Id;
            Tenant = urn.Tenant;
        }

        public static bool IsValid(StringTenantId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && string.IsNullOrWhiteSpace(aggregateRootId.Id) == false && !string.IsNullOrEmpty(aggregateRootId.Tenant);
        }
    }
}
