namespace Elders.Cronus.DomainModeling.Projections
{
    public interface IDataTransferObject<T>
    {
        T Id { get; set; }
    }
}
