namespace Elders.Cronus.DomainModeling.Projections
{
    public interface ICollectionDataTransferObjectItem<TId, VCollectionId> : ICollectionDataTransferObject<VCollectionId>
    {
        TId Id { get; set; }
    }
}