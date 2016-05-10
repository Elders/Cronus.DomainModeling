using System;
using System.Runtime.Serialization;
using System.Text;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "b3e2fc15-1996-437d-adfc-64f3b5be3244")]
    public class AggregateRootId : IAggregateRootId
    {
        protected Func<IUrn, byte[]> setRawId = (urn) => Encoding.UTF8.GetBytes(urn.Value);

        /// <summary>
        /// Prevents a default instance of the <see cref="AggregateRootId"/> class from being created.
        /// </summary>
        /// <remarks>Used only for serizalization.</remarks>
        protected AggregateRootId()
        {
            RawId = new byte[0];
            AggregateRootName = string.Empty;
        }

        protected AggregateRootId(string aggregateRootName)
        {
            if (String.IsNullOrEmpty(aggregateRootName)) throw new ArgumentNullException(nameof(aggregateRootName));

            RawId = new byte[0];
            AggregateRootName = aggregateRootName.ToLowerInvariant();
        }

        [DataMember(Order = 10)]
        public byte[] RawId { get; protected set; }

        [DataMember(Order = 11)]
        public string AggregateRootName { get; protected set; }

        public virtual IUrn Urn { get { return new Urn(AggregateRootName); } }

        public override bool Equals(System.Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!typeof(AggregateRootId).IsAssignableFrom(obj.GetType())) return false;
            return Equals((AggregateRootId)obj);
        }

        public virtual bool Equals(IAggregateRootId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ByteArrayHelper.Compare(RawId, other.RawId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCodeModifier.AggregateRootId ^ ByteArrayHelper.ComputeHash(RawId);
            }
        }

        public static bool operator ==(AggregateRootId left, AggregateRootId right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(AggregateRootId a, AggregateRootId b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return Urn.Value + "||" + Convert.ToBase64String(RawId);
        }
    }
}