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

        public GuidId(Guid idBase, string name)
        {
            if (idBase == default(Guid)) throw new ArgumentException("Default guid value is not allowed.", "idBase");
            Id = idBase;
            base.RawId = AggregateRootId.Combine(UTF8Encoding.UTF8.GetBytes(name + "@"), Id.ToByteArray());
        }

        public GuidId(GuidId idBase, string name)
        {
            if (!IsValid(idBase)) throw new ArgumentException("Default guid value is not allowed.", "idBase");
            Id = idBase.Id;
            base.RawId = AggregateRootId.Combine(UTF8Encoding.UTF8.GetBytes(name + "@"), Id.ToByteArray());
        }

        public static bool IsValid(GuidId aggregateRootId)
        {
            return (!ReferenceEquals(null, aggregateRootId)) && aggregateRootId.Id != default(Guid);
        }
    }


    // TESTS
    [DataContract(Name = "b3b879e8-b11b-4a57-8f95-0c1c7512fd73")]
    class AccountId : GuidId
    {
        public AccountId(Guid id) : base(id, "account")
        {

        }
    }


    class FriendShipId : AggregateRootId
    {
        public FriendShipId(AccountId id, AccountId friend)
        {
            RawId = AggregateRootId.Combine(id.RawId, friend.RawId);
        }
    }

}