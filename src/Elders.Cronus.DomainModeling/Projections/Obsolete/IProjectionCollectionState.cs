namespace Elders.Cronus.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IProjectionCollectionState : IProjectionState
    {
        object CollectionId { get; }
    }
}