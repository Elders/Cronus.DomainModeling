namespace Elders.Cronus;

public abstract class Entity<TAggregateRoot, TEntityState> : IEntity
    where TAggregateRoot : IAggregateRoot
    where TEntityState : IEntityState, new()
{
    private readonly TAggregateRoot root;

    protected TEntityState state;

    IEntityState IHaveState<IEntityState>.State { get { return state; } }

    protected Entity(TAggregateRoot root, EntityId entityId)
    {
        this.root = root;
        state = new TEntityState();
        var dynamicState = (dynamic)state;
        dynamicState.EntityId = (dynamic)entityId;
        foreach (var handlerAction in DomainObjectEventHandlerMapping.GetEventHandlers(() => state))
        {
            this.root.RegisterEventHandler(state.EntityId, handlerAction.Key, handlerAction.Value);
        }
    }

    protected void Apply(IEvent @event)
    {
        var entityEvent = new EntityEvent(state.EntityId, @event);
        var ar = (dynamic)root;
        ar.Apply((dynamic)entityEvent);
    }

    protected void Apply(IPublicEvent @event)
    {
        var ar = (dynamic)root;
        ar.Apply((dynamic)@event);
    }
}
