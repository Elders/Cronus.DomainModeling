using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRoot : ICanRestoreStateFromEvents<IAggregateRootState>
    {
        int Revision { get; }
        IEnumerable<IEvent> UncommittedEvents { get; }
    }
}