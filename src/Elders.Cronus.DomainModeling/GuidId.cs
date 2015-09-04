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
}