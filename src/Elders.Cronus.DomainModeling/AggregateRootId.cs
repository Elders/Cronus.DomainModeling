using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "b3e2fc15-1996-437d-adfc-64f3b5be3244")]
    public class AggregateRootId : IAggregateRootId
    {
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
            if (String.IsNullOrEmpty(aggregateRootName)) throw new ArgumentNullException("aggregateRootName");

            RawId = new byte[0];
            AggregateRootName = aggregateRootName;
        }

        [DataMember(Order = 10)]
        public byte[] RawId { get; protected set; }

        [DataMember(Order = 11)]
        public string AggregateRootName { get; protected set; }

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
            return ByteArrayCompare(RawId, other.RawId);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCodeModifier.AggregateRootId ^ ComputeHash(RawId);
            }
        }

        public static int ComputeHash(params byte[] data)
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < data.Length; i++)
                    hash = (hash ^ data[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;
                return hash;
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
            return Convert.ToBase64String(RawId);
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);

        static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            // Validate buffers are the same length.
            // This also ensures that the count does not exceed the length of either buffer.  
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] rv = new byte[first.Length + second.Length];
            System.Buffer.BlockCopy(first, 0, rv, 0, first.Length);
            System.Buffer.BlockCopy(second, 0, rv, first.Length, second.Length);
            return rv;
        }
    }
}