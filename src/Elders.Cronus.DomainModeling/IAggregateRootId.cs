using System;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootId : IBlobId, IEquatable<IAggregateRootId>
    {
        string AggregateRootName { get; }
    }
}