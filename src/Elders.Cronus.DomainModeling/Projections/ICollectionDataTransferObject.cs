namespace Elders.Cronus.DomainModeling.Projections
{
    public interface ICollectionDataTransferObject<VCollectionId>
    {
        VCollectionId CollectionId { get; set; }
    }
}
