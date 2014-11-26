using System;
using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootState : IEqualityComparer<IAggregateRootState>, IEquatable<IAggregateRootState>
    {
        IAggregateRootId Id { get; }
        void Apply(IEvent @event);
    }
}