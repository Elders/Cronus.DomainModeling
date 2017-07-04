namespace Elders.Cronus.DomainModeling.Projections
{
    [System.Obsolete("Use event sourced projections instead.")]
    public interface ICollectionDataTransferObjectItem<TId, VCollectionId> : ICollectionDataTransferObject<VCollectionId>
    {
    }
}