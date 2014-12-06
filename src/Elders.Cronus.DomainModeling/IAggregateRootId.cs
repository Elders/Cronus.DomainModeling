using System;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootId : IEquatable<IAggregateRootId>
    {
        byte[] RawId { get; }

        string AggregateRootName { get; }
    }
}