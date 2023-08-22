using System;
using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus.Testing;

public static class AggregateRootExtensions
{
    public static T PublishedEvent<T>(this IAggregateRoot root, Func<T, bool> filter = null) where T : IEvent
    {
        if (filter is not null)
        {
            var @event = PublishedEvents<T>(root).Where(filter).SingleOrDefault();
            if (@event is null)
                return default;
            return (T)@event;
        }
        else
        {
            var @event = PublishedEvents<T>(root).SingleOrDefault();
            if (@event is null)
                return default;
            return (T)@event;
        }
    }

    public static IEnumerable<T> PublishedEvents<T>(this IAggregateRoot root) where T : IEvent
    {
        var arEvents = root.UncommittedEvents.Where(x => x is T).Select(x => (T)x);

        var entityEvents = root.UncommittedEvents.Where(x => x is EntityEvent).Select(x => (EntityEvent)x);

        return arEvents.Union(entityEvents.Where(x => x.Event is T).Select(x => (T)x.Event));
    }

    public static bool IsEventPublished<T>(this IAggregateRoot root) where T : IEvent
    {
        return PublishedEvents<T>(root).Any();
    }

    public static bool HasNewEvents(this IAggregateRoot root)
    {
        return root.UncommittedEvents.Any();
    }

    public static bool HasNewPublicEvents(this IAggregateRoot root)
    {
        return root.UncommittedPublicEvents.Any();
    }

    public static T RootState<T>(this AggregateRoot<T> root) where T : IAggregateRootState, new()
    {
        return (T)(root as IHaveState<IAggregateRootState>).State;
    }
}
