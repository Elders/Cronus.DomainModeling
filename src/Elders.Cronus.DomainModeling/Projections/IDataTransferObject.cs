namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IDataTransferObject<T> : IProjectionState
    {
        T Id { get; set; }
    }
}
