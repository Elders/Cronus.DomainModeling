using System;
using System.Collections.Generic;

namespace Elders.Cronus.DomainModeling
{
    public class AggregateRoot<TState> : IAggregateRoot
        where TState : IAggregateRootState, new()
    {
        protected TState state;
        protected List<IEvent> uncommittedEvents;
        private int revision;
        private EventHandlerRegistrations handlers;

        public AggregateRoot()
        {
            state = InitializeState();
            uncommittedEvents = new List<IEvent>();
            revision = 0;

            var mapping = new DomainObjectEventHandlerMapping();
            handlers = new EventHandlerRegistrations();
            var arHandlers = mapping.GetEventHandlers(() => this.state);
            foreach (var handler in arHandlers)
            {
                handlers.Register(handler.Key, handler.Value);
            }
        }

        public TState InitializeState()
        {
            var state = new TState();
            var dynamicState = (dynamic)state;
            dynamicState.Root = (dynamic)this;
            return state;
        }

        IAggregateRootState IHaveState<IAggregateRootState>.State { get { return state; } }

        int IAggregateRoot.Revision { get { return uncommittedEvents.Count > 0 ? revision + 1 : revision; } }

        internal protected void Apply(IEvent @event)
        {
            var handler = handlers.GetEventHandler(@event);
            handler(@event);
            uncommittedEvents.Add(@event);
        }

        IEnumerable<IEvent> IAmEventSourced.UncommittedEvents { get { return uncommittedEvents.AsReadOnly(); } }

        void IAmEventSourced.ReplayEvents(List<IEvent> events, int revision)
        {
            state = InitializeState();
            foreach (IEvent @event in events)
            {
                var handler = handlers.GetEventHandler(@event);
                handler(@event);
            }
            this.revision = revision;

            if (state.Id == null || state.Id.RawId == default(byte[]))
                throw new AggregateRootException("Invalid aggregate root state. The initial event which created the aggregate root is missing.");
        }

        void IAmEventSourced.RegisterEventHandler(Type eventType, Action<IEvent> handleAction)
        {
            handlers.Register(eventType, handleAction);
        }

        void IAmEventSourced.RegisterEventHandler(IEntityId entityId, Type eventType, Action<IEvent> handleAction)
        {
            handlers.Register(entityId, eventType, handleAction);
        }


    }

    public class EventHandlerRegistrations
    {
        private Dictionary<Type, Action<IEvent>> aggregateRootHandlers;
        private Dictionary<IEntityId, Dictionary<Type, Action<IEvent>>> entityHandlers;

        public EventHandlerRegistrations()
        {
            aggregateRootHandlers = new Dictionary<Type, Action<IEvent>>();
            entityHandlers = new Dictionary<IEntityId, Dictionary<Type, Action<IEvent>>>();
        }

        public void Register(Type eventType, Action<IEvent> handler)
        {
            aggregateRootHandlers.Add(eventType, handler);
        }

        public void Register(IEntityId entityId, Type eventType, Action<IEvent> handler)
        {
            Dictionary<Type, Action<IEvent>> specificEntityHandlers;
            if (entityHandlers.TryGetValue(entityId, out specificEntityHandlers))
            {
                if (specificEntityHandlers.ContainsKey(eventType) == false)
                    specificEntityHandlers.Add(eventType, handler);
            }
            else
            {
                entityHandlers.Add(entityId, new Dictionary<Type, Action<IEvent>>() { { eventType, handler } });
            }
        }

        public Action<IEvent> GetEventHandler(IEvent @event)
        {
            var entityEvent = @event as EntityEvent;
            if (ReferenceEquals(null, entityEvent))
            {
                return aggregateRootHandlers[@event.GetType()];
            }
            else
            {
                return entityHandlers[entityEvent.EntityId][entityEvent.Event.GetType()];
            }
        }
    }

    public class EntityEvent : IEvent
    {
        EntityEvent() { }

        public EntityEvent(IEntityId id, IEvent @event)
        {
            this.EntityId = id;
            this.Event = @event;
        }

        public IEntityId EntityId { get; private set; }

        public IEvent Event { get; private set; }
    }
}