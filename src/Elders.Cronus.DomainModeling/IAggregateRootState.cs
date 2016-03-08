namespace Elders.Cronus.DomainModeling
{
    public interface IAggregateRootState
    {
        IAggregateRootId Id { get; }
    }
}