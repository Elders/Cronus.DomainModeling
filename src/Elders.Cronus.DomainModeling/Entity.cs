namespace Elders.Cronus.DomainModeling
{
    public abstract class Entity<TAggregateRoot, TEntityState>
        where TAggregateRoot : IAggregateRoot
        where TEntityState : IEntityState, new()
    {
        private readonly TAggregateRoot root;

        protected TEntityState state;

        protected Entity(TAggregateRoot root)
        {
            this.root = root;
            this.state = new TEntityState();
            var mapping = new DomainObjectEventHandlerMapping();
            foreach (var handlerAction in mapping.GetEventHandlers(() => this.state))
            {
                root.RegisterEventHandler(handlerAction.Key, handlerAction.Value);
            }
        }

        protected void Apply(IEvent @event)
        {
            var ar = (dynamic)root;
            ar.Apply((dynamic)@event);
        }
    }
}
