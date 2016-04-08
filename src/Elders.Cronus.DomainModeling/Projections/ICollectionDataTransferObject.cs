namespace Elders.Cronus.DomainModeling.Projections
{
    public interface ICollectionDataTransferObject<VCollectionId> : IProjectionCollectionState
    {
        VCollectionId CollectionId { get; set; }
    }
}
