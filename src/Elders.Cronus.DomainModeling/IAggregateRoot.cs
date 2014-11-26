using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRoot : IAggregateRootStateManager
    {
        int Revision { get; }
        void Apply(IEvent @event);
        IEnumerable<IEvent> UncommittedEvents { get; }
    }
}