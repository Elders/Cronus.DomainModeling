namespace Elders.Cronus;

public abstract class EntityState<TEntityId> : IEntityState
    where TEntityId : EntityId
{
    EntityId IEntityState.EntityId { get { return EntityId; } }

    public abstract TEntityId EntityId { get; set; }
}
