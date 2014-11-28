using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRoot : IAggregateRootStateManager
    {
        int Revision { get; }
        IEnumerable<IEvent> UncommittedEvents { get; }
    }
}