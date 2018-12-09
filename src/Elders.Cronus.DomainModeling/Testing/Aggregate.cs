using System;
using System.Collections.Generic;

namespace Elders.Cronus.Testing
{
    public static class Aggregate<T> where T : IAggregateRoot
    {
        public static AggregateRootHistory FromHistory()
        {
            return new AggregateRootHistory();
        }

        public class AggregateRootHistory
        {
            public AggregateRootHistory()
            {
                Events = new List<IEvent>();
            }

            public List<IEvent> Events { get; set; }

            public AggregateRootHistory Event(IEvent @event)
            {
                Events.Add(@event);
                return this;
            }

            public T Build()
            {
                var instance = (T)Activator.CreateInstance(typeof(T), true);
                instance.ReplayEvents(Events, 1);
                return instance;
            }
        }
    }
}
