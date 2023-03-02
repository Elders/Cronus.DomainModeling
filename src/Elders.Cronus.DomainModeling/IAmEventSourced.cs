using System;
using System.Collections.Generic;

namespace Elders.Cronus;

public interface IAmEventSourced
{
    void ReplayEvents(List<IEvent> events, int currentRevision);
    void ReplayEvents(List<IEvent> events, int currentRevision, IAggregateRootState snapshot);
    void RegisterEventHandler(Type eventType, Action<IEvent> handleAction);
    void RegisterEventHandler(EntityId entityId, Type eventType, Action<IEvent> handleAction);
    IEnumerable<IEvent> UncommittedEvents { get; }
}
