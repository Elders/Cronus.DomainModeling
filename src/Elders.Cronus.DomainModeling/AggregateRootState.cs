namespace Elders.Cronus;

public abstract class AggregateRootState<TAggregateRoot, TAggregateRootId> : IAggregateRootState
    where TAggregateRoot : IAggregateRoot
    where TAggregateRootId : AggregateRootId
{
    AggregateRootId IAggregateRootState.Id { get { return Id; } }

    public abstract TAggregateRootId Id { get; set; }

    public TAggregateRoot Root { get; set; }
}
