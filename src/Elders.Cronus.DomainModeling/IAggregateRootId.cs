using System;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootId : IHaveUrn, IBlobId, IEquatable<IAggregateRootId>
    {
        string AggregateRootName { get; }
    }
}
