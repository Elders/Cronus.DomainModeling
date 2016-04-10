namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IProjectionCollectionState : IProjectionState
    {
        object CollectionId { get; }
    }
}