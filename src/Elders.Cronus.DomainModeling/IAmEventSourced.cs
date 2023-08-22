using System;
using System.Collections.Generic;

namespace Elders.Cronus;

public interface IAmEventSourced
{
    void ReplayEvents(IEnumerable<IEvent> events, int currentRevision);
    void RegisterEventHandler(Type eventType, Action<IEvent> handleAction);
    void RegisterEventHandler(EntityId entityId, Type eventType, Action<IEvent> handleAction);
    IEnumerable<IEvent> UncommittedEvents { get; }
}
