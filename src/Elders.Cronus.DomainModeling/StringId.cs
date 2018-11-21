using System.Runtime.Serialization;

namespace Elders.Cronus
{
    [DataContract(Name = "08fe27ca-411e-45ce-94ce-5d64c45eae6c")]
    public class StringId : AggregateRootId
    {
        [DataMember(Order = 1)]
        public string Id { get; protected set; }

        protected StringId() { }

        public StringId(string idBase, string aggregateRootName) : base(aggregateRootName, new Urn(aggregateRootName, idBase))
        {
            Id = idBase;
        }

        public StringId(StringId idBase, string aggregateRootName) : this(aggregateRootName, idBase.Id) { }

        public static bool IsValid(StringId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && string.IsNullOrWhiteSpace(aggregateRootId.Id) == false;
        }
    }
}
