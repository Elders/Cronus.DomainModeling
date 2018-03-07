//using System;
//using System.Runtime.Serialization;

//namespace Elders.Cronus.DomainModeling
//{
//    [DataContract(Name = "6b2e2276-5b1a-4236-8644-ed64b3a98098")]
//    public class EntityGuidId<TAggregateRootId> : EntityId<TAggregateRootId> where TAggregateRootId : IAggregateRootId
//    {
//        [DataMember(Order = 1)]
//        public Guid Id { get; private set; }

//        protected EntityGuidId() { }

//        public EntityGuidId(Guid idBase, TAggregateRootId rootId, string entityName) : base(rootId, entityName)
//        {
//            if (idBase == default(Guid)) throw new ArgumentException("Default guid value is not allowed.", nameof(idBase));
//            Id = idBase;
//            RawId = setRawId(Urn);
//        }

//        public EntityGuidId(EntityGuidId<TAggregateRootId> idBase, string entityId) : base(idBase.RootId, entityId)
//        {
//            if (!IsValid(idBase)) throw new ArgumentException("Default guid value is not allowed.", nameof(idBase));
//            Id = idBase.Id;
//            RawId = setRawId(Urn);
//        }

//        public override IUrn Urn
//        {
//            get { return Elders.Cronus.Urn.Parse(RootId.Urn.Value + ":" + EntityName + ":" + Id.ToString()); }
//        }

//        public static bool IsValid(EntityGuidId<TAggregateRootId> entityId)
//        {
//            return (!ReferenceEquals(null, entityId)) && entityId.Id != default(Guid);
//        }
//    }
//}
