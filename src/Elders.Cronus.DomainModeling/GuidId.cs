using System;
using System.Runtime.Serialization;
using System.Text;

namespace Elders.Cronus.DomainModeling
{
    [DataContract(Name = "b3b879e8-b11b-4a57-8f95-0c1c7512fd73")]
    public class GuidId : AggregateRootId
    {
        [DataMember(Order = 2)]
        public Guid Id { get; private set; }

        public GuidId(Guid idBase)
        {
            if (idBase == default(Guid)) throw new ArgumentException("Default guid value is not allowed.", "idBase");
            Id = idBase;
            base.RawId = AggregateRootId.Combine(UTF8Encoding.UTF8.GetBytes(this.GetType().Name + "="), Id.ToByteArray());
        }

        public GuidId(GuidId idBase)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Default guid value is not allowed.", "idBase");
            Id = idBase.Id;
            base.RawId = AggregateRootId.Combine(UTF8Encoding.UTF8.GetBytes(this.GetType().Name + "="), Id.ToByteArray());
        }

        public static bool IsValid(GuidId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && aggregateRootId.Id != default(Guid);
        }
    }
}