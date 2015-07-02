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
            if (string.IsNullOrWhiteSpace(idBase)) throw new ArgumentException("Empty string value is not allowed.", "idBase");
            Id = idBase;
            base.RawId = ByteArrayHelper.Combine(UTF8Encoding.UTF8.GetBytes(AggregateRootName + "@"), UTF8Encoding.UTF8.GetBytes(Id));
        }

        public StringId(StringId idBase, string aggregateRootName) : base(aggregateRootName)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Empty string value is not allowed.", "idBase");
            Id = idBase.Id;
            base.RawId = ByteArrayHelper.Combine(UTF8Encoding.UTF8.GetBytes(AggregateRootName + "@"), UTF8Encoding.UTF8.GetBytes(Id));
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
}