using System;

namespace Elders.Cronus.DomainModeling
{
    public interface IEntityId : IHaveUrn, IBlobId, IEquatable<IEntityId>
    {
        string EntityName { get; }
        IAggregateRootId AggregateRootId { get; }
    }
}