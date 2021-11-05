using System;
using System.Collections.Generic;

namespace Elders.Cronus
{
    public interface IAmEventSourced
    {
        void ReplayEvents(List<IEvent> events, int currentRevision);
        void RegisterEventHandler(Type eventType, Action<IEvent> handleAction);
        void RegisterEventHandler(IEntityId entityId, Type eventType, Action<IEvent> handleAction);
        IEnumerable<IEvent> UncommittedEvents { get; }
    }
}
