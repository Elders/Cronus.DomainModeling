using System;

namespace Elders.Cronus
{
    public interface IAggregateRootId : IHaveUrn, IBlobId, IEquatable<IAggregateRootId>
    {
        string AggregateRootName { get; }
    }
}
