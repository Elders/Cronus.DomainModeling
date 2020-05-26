namespace Elders.Cronus
{
    public abstract class Entity<TAggregateRoot, TEntityState> : IEntity
        where TAggregateRoot : IAggregateRoot
        where TEntityState : IEntityState, new()
    {
        private readonly TAggregateRoot root;

        protected TEntityState state;

        IEntityState IHaveState<IEntityState>.State { get { return state; } }

        protected Entity(TAggregateRoot root, IEntityId entityId)
        {
            this.root = root;
            this.state = new TEntityState();
            var dynamicState = (dynamic)this.state;
            dynamicState.EntityId = (dynamic)entityId;
            var mapping = new DomainObjectEventHandlerMapping();
            foreach (var handlerAction in mapping.GetEventHandlers(() => this.state))
            {
                root.RegisterEventHandler(state.EntityId, handlerAction.Key, handlerAction.Value);
            }
        }

        protected void Apply(IEvent @event)
        {
            var entityEvent = new EntityEvent(state.EntityId, @event);
            var ar = (dynamic)root;
            ar.Apply((dynamic)entityEvent);
        }
    }

    public static class EntityExtensions
    {
        public static T GetState<T>(this IHaveState<T> entity) where T : IEntityState => entity.State;

        public static TEntityState State<TAggregateRoot, TEntityState>(this Entity<TAggregateRoot, TEntityState> entity)
            where TAggregateRoot : IAggregateRoot
            where TEntityState : IEntityState, new()
        {
            return (TEntityState)GetState(entity);
        }

    }
}
