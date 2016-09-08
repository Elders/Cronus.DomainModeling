using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "ac9f6c97-c1c8-486c-9647-d2a1ab44d767")]
    public class EntityStringId<TAggregateRootId> : EntityId<TAggregateRootId> where TAggregateRootId : IAggregateRootId
    {
        [DataMember(Order = 1)]
        public string Id { get; private set; }

        protected EntityStringId() { }

        public EntityStringId(string idBase, TAggregateRootId rootId, string entityName) : base(rootId, entityName)
        {
            if (string.IsNullOrWhiteSpace(idBase)) throw new ArgumentException("Empty string is not allowed.", nameof(idBase));
            Id = idBase;
            RawId = setRawId(Urn);
        }

        public EntityStringId(EntityStringId<TAggregateRootId> idBase, string entityId) : base(idBase.RootId, entityId)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Empty string is not allowed.", nameof(idBase));
            Id = idBase.Id;
            RawId = setRawId(Urn);
        }

        public override IUrn Urn
        {
            get { return DomainModeling.Urn.Parse(RootId.Urn.Value + ":" + EntityName + ":" + Id); }
        }

        public static bool IsValid(EntityStringId<TAggregateRootId> entityId)
        {
            return (!ReferenceEquals(null, entityId)) && string.IsNullOrWhiteSpace(entityId.Id) == false;
        }
    }
}