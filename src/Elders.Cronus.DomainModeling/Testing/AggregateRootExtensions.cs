using System.Collections.Generic;
using System.Linq;

namespace Elders.Cronus.Testing
{
    public static class AggregateRootExtensions
    {
        public static T PublishedEvent<T>(this IAggregateRoot root) where T : IEvent
        {
            var @event = PublishedEvents<T>(root).SingleOrDefault();
            if (@event is null)
                return default(T);
            return (T)@event;
        }

        public static IEnumerable<T> PublishedEvents<T>(this IAggregateRoot root) where T : IEvent
        {
            var arEvents = root.UncommittedEvents.Where(x => x is T).Select(x => (T)x);

            var entityEvents = root.UncommittedEvents.Where(x => x is EntityEvent).Select(x => (EntityEvent)x);

            return arEvents.Union(entityEvents.Where(x => x.Event is T).Select(x => (T)x.Event));
        }

        public static bool IsEventPublished<T>(this IAggregateRoot root) where T : IEvent
        {
            return ReferenceEquals(default(T), PublishedEvent<T>(root)) == false;
        }

        public static bool HasNewEvents(this IAggregateRoot root)
        {
            return root.UncommittedEvents.Any();
        }

        public static T RootState<T>(this AggregateRoot<T> root) where T : IAggregateRootState, new()
        {
            return (T)(root as IHaveState<IAggregateRootState>).State;
        }
    }
}
