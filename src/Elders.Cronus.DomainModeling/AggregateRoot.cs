using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elders.Cronus;

public class AggregateRoot<TState> : IAggregateRoot
    where TState : IAggregateRootState, new()
{
    protected TState state;
    protected List<IEvent> uncommittedEvents;
    protected List<IPublicEvent> uncommittedPublicEvents;
    private int revision;
    private EventHandlerRegistrations handlers;

    public AggregateRoot()
    {
        state = InitializeState();
        uncommittedEvents = new List<IEvent>();
        uncommittedPublicEvents = new List<IPublicEvent>();
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
        IEvent realEvent;
        var handler = handlers.GetEventHandler(@event, out realEvent);
        handler(realEvent);
        uncommittedEvents.Add(@event);
    }

    internal protected void Apply(IPublicEvent @event)
    {
        uncommittedPublicEvents.Add(@event);
    }

    IEnumerable<IEvent> IAmEventSourced.UncommittedEvents { get { return uncommittedEvents.AsReadOnly(); } }

    IEnumerable<IPublicEvent> IUnderstandPublishedLanguage.UncommittedPublicEvents { get { return uncommittedPublicEvents.AsReadOnly(); } }

    void IAmEventSourced.ReplayEvents(List<IEvent> events, int revision)
    {
        state = InitializeState();
        foreach (IEvent @event in events)
        {
            IEvent realEvent;
            var handler = handlers.GetEventHandler(@event, out realEvent);
            handler(realEvent);
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

public class EventHandlerRegistrations // internal?
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

    public Action<IEvent> GetEventHandler(IEvent @event, out IEvent realEvent)
    {
        realEvent = @event;
        var realEventType = realEvent.GetType();
        var entityEvent = @event as EntityEvent;
        Action<IEvent> stateHandler = null;

        if (ReferenceEquals(null, entityEvent))
        {
            aggregateRootHandlers.TryGetValue(realEventType, out stateHandler);
        }
        else
        {
            realEvent = entityEvent.Event;
            realEventType = realEvent.GetType();
            Dictionary<Type, Action<IEvent>> entityRegistration;
            if (entityHandlers.TryGetValue(entityEvent.EntityId, out entityRegistration))
            {
                entityRegistration.TryGetValue(realEventType, out stateHandler);
            }
        }

        if (ReferenceEquals(null, stateHandler))
        {
            string error = "State handler not found for '" + realEventType.FullName + "' in Entity/AR state.";
            throw new Exception(error);
        }
        return stateHandler;
    }
}

[DataContract(Name = "f2271a33-cf0d-4882-b68e-d4cee9ac0491")]
public class EntityEvent : IEvent
{
    EntityEvent() { }

    public EntityEvent(IEntityId id, IEvent @event)
    {
        this.EntityId = id;
        this.Event = @event;
    }

    [DataMember(Order = 1)]
    public IEntityId EntityId { get; private set; }

    [DataMember(Order = 2)]
    public IEvent Event { get; private set; }
}
