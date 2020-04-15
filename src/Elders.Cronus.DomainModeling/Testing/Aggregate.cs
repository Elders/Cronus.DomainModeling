using System;
using System.Collections.Generic;

namespace Elders.Cronus.Testing
{
    public static class Aggregate<T> where T : IAggregateRoot
    {
        public static T FromHistory(Action<AggregateRootHistory> stream)
        {
            var history = new AggregateRootHistory();
            stream(history);
            return history.Build();
        }

        public class AggregateRootHistory
        {
            public AggregateRootHistory()
            {
                Events = new List<IEvent>();
            }

            public List<IEvent> Events { get; private set; }

            public AggregateRootHistory AddEvent(IEvent @event)
            {
                if (@event is null) throw new ArgumentNullException(nameof(@event));

                Events.Add(@event);
                return this;
            }

            internal T Build()
            {
                var instance = (T)Activator.CreateInstance(typeof(T), true);
                instance.ReplayEvents(Events, 1);
                return instance;
            }
        }
    }
}
