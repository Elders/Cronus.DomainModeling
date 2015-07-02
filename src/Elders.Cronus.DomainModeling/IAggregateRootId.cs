using System;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootId : IEquatable<IAggregateRootId>
    {
        string AggregateRootName { get; }
        byte[] RawId { get; }
    }
}