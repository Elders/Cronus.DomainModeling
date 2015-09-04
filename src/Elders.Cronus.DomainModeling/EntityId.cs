using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "44f705a4-f339-4677-b39a-300a9eaa4a73")]
    public class EntityId<TAggregateRootId> : IEntityId
        where TAggregateRootId : IAggregateRootId
    {
        protected EntityId()
        {
            RawId = new byte[0];
            EntityName = string.Empty;
        }

        protected EntityId(TAggregateRootId rootId, string entityName)
        {
            if (ReferenceEquals(null, rootId)) throw new ArgumentNullException(nameof(rootId));
            if (String.IsNullOrEmpty(entityName)) throw new ArgumentNullException(nameof(entityName));

            RawId = new byte[0];
            EntityName = entityName;
            RootId = rootId;
        }

        [DataMember(Order = 20)]
        public byte[] RawId { get; protected set; }

        [DataMember(Order = 21)]
        public string EntityName { get; protected set; }

        [DataMember(Order = 22)]
        public TAggregateRootId RootId { get; set; }

        IAggregateRootId IEntityId.AggregateRootId { get { return RootId; } }

        public override bool Equals(System.Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!typeof(IEntityId).IsAssignableFrom(obj.GetType())) return false;
            return Equals((EntityId<TAggregateRootId>)obj);
        }

        public virtual bool Equals(IEntityId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ByteArrayHelper.Compare(RawId, other.RawId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCodeModifier.EntityId ^ ByteArrayHelper.ComputeHash(RawId);
            }
        }

        public static bool operator ==(EntityId<TAggregateRootId> left, EntityId<TAggregateRootId> right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(EntityId<TAggregateRootId> a, EntityId<TAggregateRootId> b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return Convert.ToBase64String(RawId);
        }
    }
}