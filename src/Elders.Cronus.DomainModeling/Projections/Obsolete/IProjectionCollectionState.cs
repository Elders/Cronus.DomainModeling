namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface IProjectionCollectionState : IProjectionState
    {
        object CollectionId { get; }
    }
}