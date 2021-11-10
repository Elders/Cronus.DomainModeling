namespace Elders.Cronus;

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
