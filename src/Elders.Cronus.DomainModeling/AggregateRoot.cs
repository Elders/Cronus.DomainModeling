using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Elders.Cronus.DomainModeling
{
    public class AggregateRoot<TState> : IAggregateRoot
        where TState : IAggregateRootState, new()
    {
        protected TState state;
        protected List<IEvent> uncommittedEvents;
        private int revision;
        private Dictionary<Type, Action<IEvent>> handlers;

        public AggregateRoot()
        {
            state = new TState();
            var dynamicRoot = (dynamic)state;
            dynamicRoot.Root = (dynamic)this;
            uncommittedEvents = new List<IEvent>();
            revision = 0;

            var mapping = new DomainObjectEventHandlerMapping();
            handlers = mapping.GetEventHandlers(() => this.state);
        }

        IAggregateRootState ICanRestoreStateFromEvents<IAggregateRootState>.State { get { return state; } }

        IEnumerable<IEvent> IAggregateRoot.UncommittedEvents { get { return uncommittedEvents.AsReadOnly(); } }

        int IAggregateRoot.Revision { get { return uncommittedEvents.Count > 0 ? revision + 1 : revision; } }

        void ICanRestoreStateFromEvents<IAggregateRootState>.ReplayEvents(List<IEvent> events, int revision)
        {
            state = new TState();
            foreach (IEvent @event in events)
            {
                var handler = handlers[@event.GetType()];
                handler(@event);
            }
            this.revision = revision;

            if (state.Id == null || state.Id.RawId == default(byte[]))
                throw new AggregateRootException("Invalid aggregate root state. The initial event which created the aggregate root is missing.");
        }

        internal protected void Apply(IEvent @event)
        {
            var handler = handlers[@event.GetType()];
            handler(@event);
            uncommittedEvents.Add(@event);
        }

        void ICanRestoreStateFromEvents<IAggregateRootState>.RegisterEventHandler(Type eventType, Action<IEvent> handleAction)
        {
            handlers.Add(eventType, handleAction);
        }
    }

    public class DomainObjectEventHandlerMapping
    {
        public Dictionary<Type, Action<IEvent>> GetEventHandlers(Func<object> target)
        {
            var targetType = target().GetType();
            var handlers = new Dictionary<Type, Action<IEvent>>();

            var methodsToMatch = targetType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var matchedMethods = from method in methodsToMatch
                                 let parameters = method.GetParameters()
                                 where
                                    method.Name.Equals("when", StringComparison.InvariantCultureIgnoreCase) &&
                                    parameters.Length == 1 &&
                                    typeof(IEvent).IsAssignableFrom(parameters[0].ParameterType)
                                 select
                                    new { MethodInfo = method, FirstParameter = method.GetParameters()[0] };

            foreach (var method in matchedMethods)
            {
                var methodCopy = method.MethodInfo;
                Type eventType = methodCopy.GetParameters().First().ParameterType;

                Action<IEvent> handler = (e) => methodCopy.Invoke(target(), new[] { e });

                handlers.Add(eventType, handler);
            }

            return handlers;
        }
    }
}