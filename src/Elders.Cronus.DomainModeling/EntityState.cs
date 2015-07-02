namespace Elders.Cronus.DomainModeling
{
    public abstract class EntityState<TEntityId> : IEntityState
        where TEntityId : IEntityId
    {
        IEntityId IEntityState.EntityId { get { return EntityId; } }

        public abstract TEntityId EntityId { get; set; }
    }
}