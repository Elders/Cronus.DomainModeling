using System;

namespace Elders.Cronus
{
    public interface IEntityId : IHaveUrn, IBlobId, IEquatable<IEntityId>
    {
        string EntityName { get; }
        IAggregateRootId AggregateRootId { get; }
    }
}
