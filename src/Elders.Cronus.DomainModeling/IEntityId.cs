using System;
using System.Runtime.Serialization;

namespace Elders.Cronus.DomainModeling
{
    public interface IEntityId : IBlobId, IEquatable<IEntityId>
    {
        string EntityName { get; }
        IAggregateRootId AggregateRootId { get; }
    }
}