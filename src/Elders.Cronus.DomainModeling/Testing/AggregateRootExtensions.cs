using System.Linq;

namespace Elders.Cronus.Testing
{
    public static class AggregateRootExtensions
    {
        public static T PublishedEvent<T>(this IAggregateRoot root) where T : IEvent
        {
            var @event = root.UncommittedEvents.SingleOrDefault(x => x is T);
            if (ReferenceEquals(@event, null) || @event.Equals(default(T)))
                return default(T);
            return (T)@event;
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
